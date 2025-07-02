using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AirRushAction", story: "Air Rush Action", category: "Action", id: "df3b8ffe0ffae7adeaba224d5d92fdc7")]
public partial class AirRushAction : Action
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

        direction = (enemy.player.transform.position - enemy.transform.position).normalized;

        if (enemy.player.rb.linearVelocity.x != 0)
            direction = new Vector3(direction.x + .15f * enemy.player.facingDir, direction.y);

        canUltimate.Value = false;

        enemy.canFip = false;

        enemy.stats.MakeInvincible(true);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        rb.linearVelocity = direction * enemy.speed;

        if (enemy.CanNotUltimate())
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

