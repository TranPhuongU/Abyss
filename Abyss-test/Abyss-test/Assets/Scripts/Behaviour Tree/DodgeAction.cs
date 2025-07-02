using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DodgeAction", story: "[Dodge] Action", category: "Action", id: "bb94ffa6f0a74966cda8cea7d762ad6c")]
public partial class DodgeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> enemyGO;
    [SerializeReference] public BlackboardVariable<bool> animationTrigger;

    private Enemy_CrimsonBloom enemy;
    protected override Status OnStart()
    {

        if (enemyGO.Value == null)
            return Status.Failure;

        enemy = enemyGO.Value.GetComponent<Enemy_CrimsonBloom>();

        if (enemy == null)
            return Status.Failure;

        enemy.canFip = false;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        enemy.rb.linearVelocity = new Vector2(-enemy.facingDir * 20, 0);

        if (animationTrigger.Value)
        {
            return Status.Success;

        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        animationTrigger.Value = false;

        enemy.canFip = true;
    }
}

