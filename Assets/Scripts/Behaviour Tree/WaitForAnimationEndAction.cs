using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForAnimationEnd", story: "Wait For Animation End on [animationTrigger]", category: "Action", id: "007cf49056e7d51550048deab0813c47")]
public partial class WaitForAnimationEndAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> animationTrigger;


    [SerializeReference] public BlackboardVariable<bool> shield;

    [SerializeField] private float timeout = 5f; // Thời gian tối đa chờ animation
    private float timer;



    protected override Status OnStart()
    {
        if (animationTrigger != null)
        {
            animationTrigger.Value = false;
            Debug.Log("WaitForAnimationEnd: Started - Reset trigger to false");
        }

        timer = timeout;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        timer -= Time.deltaTime;

        if (animationTrigger.Value)
        {
            return Status.Success;
        }

        if (shield.Value)
        {
            return Status.Success;
        }

        if (timer <= 0f)
        {
            Debug.LogWarning("WaitForAnimationEnd: Timeout fallback triggered!");
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}