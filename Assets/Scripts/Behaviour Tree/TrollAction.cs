using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TrollAction", story: "Troll Action", category: "Action", id: "d23dc5f11874cd9953f22118aadd73b9")]
public partial class TrollAction : Action
{

    [SerializeReference] public BlackboardVariable<GameObject> enemyGO;

    [SerializeReference] public BlackboardVariable<bool> animationTrigger;

    private Enemy_CrimsonBloom enemy;
    [SerializeReference] public BlackboardVariable<bool> playerDashed;
    private float stateTimer;

    protected override Status OnStart()
    {
        if (enemyGO.Value == null)
            return Status.Failure;

        enemy = enemyGO.Value.GetComponent<Enemy_CrimsonBloom>();

        if (enemy == null || enemy.player == null)
            return Status.Failure;

        playerDashed.Value = false;
        stateTimer = 0.1f;
        enemy.canFip = true;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (enemy == null || enemy.player == null)
            return Status.Failure;

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
            enemy.canFip = false;

        if (enemy.player.dashing)
            playerDashed.Value = true;

        // Kết thúc animation sẽ được trigger bởi Event trong Animator
        if (animationTrigger.Value)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (enemy != null)
        {
            enemy.canFip = true;
        }

        if (animationTrigger != null) animationTrigger.Value = false;

    }
}

