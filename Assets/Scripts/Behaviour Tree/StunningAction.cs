using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StunningAction", story: "Stunning Action", category: "Action", id: "fe78d3a163793d9afa23bb7b6907e007")]
public partial class StunningAction : Action
{

    [SerializeReference] public BlackboardVariable<GameObject> enemyGO;

    private Enemy_CrimsonBloom enemy;
    private Rigidbody2D rb;

    protected override Status OnStart()
    {
        if (enemyGO.Value == null)
            return Status.Failure;

        enemy = enemyGO.Value.GetComponent<Enemy_CrimsonBloom>();
        rb = enemy?.GetComponent<Rigidbody2D>();

        if (enemy == null || rb == null)
            return Status.Failure;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Knockback theo hướng ngược lại
        rb.linearVelocity = Vector2.right * -enemy.facingDir * 25f;

        // Kiểm tra nếu bị choáng
        if (enemy.Stunned())
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (enemy != null)
        {
            enemy.SetZeroVelocity(); // Hoặc bạn có thể cho dừng lại nếu muốn
        }
    }
}

