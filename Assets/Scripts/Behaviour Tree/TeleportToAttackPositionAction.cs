using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Teleport To Attack Position", story: "Teleport [Self] to attack position near [Player] with Y offset [TeleportYOffset]", category: "Action", id: "teleport_attack_position_001")]
public partial class TeleportToAttackPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<GameObject> Self; // Boss GameObject

    // Expose these to Blackboard for customization
    [SerializeReference] public BlackboardVariable<float> MovingPlayerOffset;
    [SerializeReference] public BlackboardVariable<float> StillPlayerOffset;

    // NEW: Y offset for teleport position
    [SerializeReference] public BlackboardVariable<float> TeleportYOffset;

    // HIDDEN: MaxAttempts with default value of 10
    private const int MAX_ATTEMPTS = 10;

    private Enemy_CrimsonBloom bossScript;
    private bool teleportCompleted = false;

    protected override Status OnStart()
    {
        if (Player?.Value == null || Self?.Value == null)
        {
            Debug.LogError("Player or Self reference is null!");
            return Status.Success;
        }

        // Get boss script
        bossScript = Self.Value.GetComponent<Enemy_CrimsonBloom>();
        if (bossScript == null)
        {
            Debug.LogError("Boss doesn't have Enemy_CrimsonBloom component!");
            return Status.Success;
        }

        teleportCompleted = false;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        if (!teleportCompleted)
        {
            bool positionFound = FindAttackPosition();
            teleportCompleted = true;

            if (positionFound)
            {
                return Status.Success; // Hoàn thành thành công, chuyển sang node tiếp theo
            }
            else
            {
                return Status.Success; // Thất bại
            }
        }

        return Status.Success;
    }

    private bool FindAttackPosition()
    {
        GameObject player = Player.Value;
        Transform bossTransform = Self.Value.transform;
        Collider2D bossCollider = Self.Value.GetComponent<Collider2D>();

        float yOffset = TeleportYOffset?.Value ?? 0f; // Get Y offset from blackboard, default to 0

        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            float xOffset = CalculateXOffset();

            // Calculate target Y position based on Y offset
            float targetY;
            if (Mathf.Approximately(yOffset, 0f))
            {
                // Original behavior: Y = player Y
                targetY = player.transform.position.y;
            }
            else
            {
                // Custom Y offset: Y = player Y + offset
                targetY = player.transform.position.y + yOffset;
            }

            // Set vị trí với Y tùy chỉnh, X = player X + offset
            Vector3 testPos = new Vector3(
                player.transform.position.x + xOffset,
                targetY
            );

            bossTransform.position = testPos;

            // Nếu Y offset = 0, sử dụng logic gốc với ground detection
            if (Mathf.Approximately(yOffset, 0f))
            {
                // Sử dụng hàm GroundBelow() của boss
                RaycastHit2D groundHit = bossScript.GroundBelow();

                if (groundHit.collider != null)
                {
                    // Canh đúng mặt đất
                    float groundY = testPos.y - groundHit.distance + (bossCollider.bounds.size.y / 2f);
                    bossTransform.position = new Vector3(testPos.x, groundY);

                    // Sử dụng hàm SomethingIsAround() của boss
                    if (!bossScript.SomethingIsAround())
                    {
                        Debug.Log($"Boss teleported to attack position (ground-aligned): {bossTransform.position}");
                        return true;
                    }
                }
            }
            else
            {
                // Custom Y offset: không cần ground detection, chỉ check collision
                bossTransform.position = testPos;

                // Sử dụng hàm SomethingIsAround() của boss
                if (!bossScript.SomethingIsAround())
                {
                    Debug.Log($"Boss teleported to attack position (custom Y offset {yOffset}): {bossTransform.position}");
                    return true;
                }
            }
        }

        Debug.LogWarning("Boss couldn't find valid attack position after multiple attempts.");
        return false;
    }

    private float CalculateXOffset()
    {
        float playerInputDirection = GetPlayerInputDirection();

        // Get values from blackboard or use defaults
        float movingOffset = MovingPlayerOffset?.Value ?? 2.5f;
        float stillOffset = StillPlayerOffset?.Value ?? 1.6f;

        // If player is giving input (moving)
        if (Mathf.Abs(playerInputDirection) > 0.1f)
        {
            // Position ahead of player's movement direction
            if (!bossScript.SomethingIsAround())
            {
                // Position in player's input direction (intercept path)
                return playerInputDirection * movingOffset;
            }
            else
            {
                // Position opposite to player's input direction (behind)
                return -playerInputDirection * movingOffset;
            }
        }
        else
        {
            // Player is idle - random left/right positioning
            return UnityEngine.Random.Range(0, 100) < 50 ? stillOffset : -stillOffset;
        }
    }

    private float GetPlayerInputDirection()
    {
        // Try to get xInput from current player state
        var playerComponent = Player.Value.GetComponent<Player>();
        if (playerComponent != null && playerComponent.stateMachine != null)
        {
            // Access xInput from current state
            var currentState = playerComponent.stateMachine.currentState;
            if (currentState != null)
            {
                return currentState.xInput;
            }
        }

        // Fallback 1: Try direct input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            return horizontalInput;
        }

        // Fallback 2: Use facing direction
        if (playerComponent != null)
        {
            return playerComponent.facingDir;
        }

        return 0f; // No movement detected
    }
}