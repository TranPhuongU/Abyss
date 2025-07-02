using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckPlayerInAttackAreaAction", story: "CheckPlayerInAttackAreaAction", category: "Action", id: "9d0f628909c3373ced6e65e4e5aff210")]
public partial class CheckPlayerInAttackAreaAction : Action
{

    [SerializeReference] public BlackboardVariable<GameObject> Self;     // Boss object
    [SerializeReference] public BlackboardVariable<GameObject> Player;   // To assign Player if found

    private Enemy_CrimsonBloom bossScript;

    protected override Status OnStart()
    {
        if (Self?.Value == null)
        {
            Debug.LogError("Self reference is null in CheckPlayerInAttackAreaAction.");
            return Status.Failure;
        }

        bossScript = Self.Value.GetComponent<Enemy_CrimsonBloom>();
        if (bossScript == null)
        {
            Debug.LogError("Self does not have Enemy_CrimsonBloom component.");
            return Status.Failure;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Player.Value = bossScript.GetPlayerGameObject();

        if (bossScript.PlayerInAttackArea())
        {
            if (Player != null)
            {
                Player.Value = bossScript.GetPlayerGameObject();
                Debug.Log("✅ Player assigned to Blackboard.");
            }

            return Status.Success;
        }

        return Status.Running;
    }
}

