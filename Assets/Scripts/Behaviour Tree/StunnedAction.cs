using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StunnedAction", story: "Stunned Action", category: "Action", id: "4cd8c87bbf8bec878f518a0ca22baa08")]
public partial class StunnedAction : Action
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

        rb.gravityScale = 60;

        ((EnemyStats)enemy.stats).currentShield -= Mathf.RoundToInt(((EnemyStats)enemy.stats).maxShield * .25f);

        if (((EnemyStats)enemy.stats).onShieldChanged != null)
            ((EnemyStats)enemy.stats).onShieldChanged();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

