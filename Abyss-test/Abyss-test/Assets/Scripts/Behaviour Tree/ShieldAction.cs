using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ShieldAction", story: "Shield Action", category: "Action", id: "523fca3cb1bdc0a31582b6dea463b3a1")]
public partial class ShieldAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> enemyGO;

    private Enemy_CrimsonBloom enemy;

    [SerializeReference] public BlackboardVariable<float> shieldTimer;

    private float stateTimer;


    protected override Status OnStart()
    {
        if (enemyGO.Value == null)
            return Status.Failure;

        enemy = enemyGO.Value.GetComponent<Enemy_CrimsonBloom>();

        if (enemy == null)
            return Status.Failure;

        enemy.canFip = false;

        enemy.canShield = false;

        stateTimer = shieldTimer;

        enemy.stats.magicResistance.AddModifier(Mathf.RoundToInt(enemy.stats.magicResistance.GetValue() * .5f));

        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        stateTimer -= Time.deltaTime;

        if (stateTimer < 0)
        {
            ((EnemyStats)enemy.stats).currentShield += Mathf.RoundToInt(((EnemyStats)enemy.stats).maxShield * .1f);

            if (((EnemyStats)enemy.stats).onShieldChanged != null)
                ((EnemyStats)enemy.stats).onShieldChanged();

            stateTimer = shieldTimer;
        }

        if (((EnemyStats)enemy.stats).currentShield >= ((EnemyStats)enemy.stats).maxShield)
        {
            return Status.Success;
        }

        return Status.Running;
        
    }

    protected override void OnEnd()
    {
        enemy.canShield = true;

        enemy.canFip = true;

        enemy.stats.magicResistance.RemoveModifier(Mathf.RoundToInt(enemy.stats.magicResistance.baseValue * .5f));
    }
}

