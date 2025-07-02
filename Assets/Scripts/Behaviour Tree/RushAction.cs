using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RushAction", story: "Rushes towards the player", category: "Action", id: "fc543fcdaeaee2adaa843bf8eb89475d")]
public partial class RushAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> enemyGO;
    [SerializeReference] public BlackboardVariable<GameObject> playerGO;

    [SerializeReference] public BlackboardVariable<bool> canUltimate;


    private Enemy_CrimsonBloom enemy;
    private Rigidbody2D rb;
    private Vector3 direction;

    protected override Status OnStart()
    {
        if (enemyGO.Value == null || playerGO.Value == null)
            return Status.Failure;

        enemy = enemyGO.Value.GetComponent<Enemy_CrimsonBloom>();
        rb = enemy.GetComponent<Rigidbody2D>();

        if (enemy == null || rb == null)
            return Status.Failure;

        canUltimate.Value = false;

        enemy.canFip = false;

        enemy.stats.MakeInvincible(true);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        rb.linearVelocity = Vector2.right * enemy.facingDir * enemy.speed;

        if (enemy.IsWallDetected())
        {
            canUltimate.Value = false;
            return Status.Success;
        }

        if (enemy.CanUltimate() && !enemy.player.dashing)
        {
            canUltimate.Value = true;
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (enemy != null)
        {
            enemy.canFip = true;
            enemy.SetZeroVelocity();

            enemy.stats.MakeInvincible(false);
        }
    }
}
