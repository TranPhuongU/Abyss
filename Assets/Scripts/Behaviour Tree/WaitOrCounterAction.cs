using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitOrCounter", story: "Wait Or Counter", category: "Action", id: "2e75ddd31a523c430823e172d8ef0e8e")]
public partial class WaitOrCounterAction : Action
{

    [SerializeReference] public BlackboardVariable<bool> animationTrigger;
    [SerializeReference] public BlackboardVariable<bool> isCountered;
    [SerializeReference] public BlackboardVariable<bool> wasCountered;

    protected override Status OnStart()
    {
        if (animationTrigger != null) animationTrigger.Value = false;
        if (wasCountered != null) wasCountered.Value = false;

        if (isCountered != null)
            isCountered.Value = false;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (isCountered?.Value == true)
        {
            if (wasCountered != null) wasCountered.Value = true;
            return Status.Success;
        }

        if (animationTrigger?.Value == true)
        {
            if (wasCountered != null) wasCountered.Value = false;
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {

    }
}

