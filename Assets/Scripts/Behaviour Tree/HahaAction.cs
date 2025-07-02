using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "HahaAction", story: "HahaAction", category: "Action", id: "7e5a02a59d4d72306b0a4b985017f75c")]
public partial class HahaAction : Action
{

    [SerializeReference] public BlackboardVariable<GameObject> enemyGO;

    [SerializeReference] public BlackboardVariable<bool> dodge;

    private Enemy_CrimsonBloom enemy;
    private float stateTimer;
    private bool playerWasNear;

    protected override Status OnStart()
    {
        if (enemyGO.Value == null)
            return Status.Failure;

        enemy = enemyGO.Value.GetComponent<Enemy_CrimsonBloom>();

        if (enemy == null)
            return Status.Failure;

        dodge.Value = false;

        enemy.canFip = false;
        stateTimer = 2f;
        playerWasNear = false;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (enemy == null || enemy.player == null)
            return Status.Failure;

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            return Status.Success;
        }

        bool playerIsNear = Vector2.Distance(enemy.transform.position, enemy.player.transform.position) < 0.7f;

        if (playerIsNear && !playerWasNear)
        {
            // Dash gần thì tỉ lệ né cao hơn
            int dodgeChance = enemy.player.dashing ? 99 : 88;

            if (UnityEngine.Random.Range(0, 100) < dodgeChance)
            {
                dodge.Value = true;

                return Status.Success;
            }
        }

        playerWasNear = playerIsNear;
        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (enemy != null)
            enemy.canFip = true;
    }
}

