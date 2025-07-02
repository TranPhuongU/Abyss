using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Explosion1Action", story: "Explosion1 Action", category: "Action", id: "9c2840f44a6718dc9e5e9152a9706110")]
public partial class Explosion1Action : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    [SerializeReference] public BlackboardVariable<bool> canBasicAttack;

    [SerializeReference] public BlackboardVariable<float> jumpHeight ;
    [SerializeReference] public BlackboardVariable<float> horizontalBoost;

    private Rigidbody2D rb;
    private Transform playerTransform;

    private float stateTimer = .4f;

    private Enemy_CrimsonBloom enemy;

    protected override Status OnStart()
    {
        enemy = Self.Value.GetComponent<Enemy_CrimsonBloom>();

        if (Player?.Value == null || Self?.Value == null)
            return Status.Failure;

        rb = Self.Value.GetComponent<Rigidbody2D>();
        playerTransform = Player.Value.transform;

        if (rb == null || playerTransform == null)
            return Status.Failure;

        stateTimer = .4f;

        canBasicAttack.Value = false;

        rb.linearVelocity = new Vector2(horizontalBoost * enemy.facingDir, jumpHeight);
        return Status.Running;
    }


    protected override Status OnUpdate()
    {
        enemy.anim.SetFloat("yVelocity", rb.linearVelocity.y);

        stateTimer -= Time.deltaTime;

        if (enemy.CanUltimate())
        {
            canBasicAttack.Value = true;    

            return Status.Success;
        }  
        else
        {
            if (enemy.CanNotUltimate() && stateTimer <=0 )
            {
                canBasicAttack.Value = false;
                return Status.Success;

            }
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        // Do nothing
    }
}

