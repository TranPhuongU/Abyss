using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
       // player.anim.SetBool("SuccessfulCounterAttack", false);

        player.skill.parry.CreateParryFX();
    }

    public override void Exit()
    {
        base.Exit();

        player.skill.parry.DestroyParryFX();

    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                //SuccesfulCounterAttack();
            }

            if(hit.GetComponent<DarkOrb_Controller>() != null)
            {
                hit.GetComponent<DarkOrb_Controller>().FlipDarkOrb();
            }

            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    //SuccesfulCounterAttack();
                    if (!player.isForm2)
                    {
                        player.skill.parry.SetTriggerAnim();
                    }
                    else if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                    }



                    player.skill.parry.UseSkill(); // goint to use to restore health on parry
                }
            }
        }

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    private void SuccesfulCounterAttack()
    {
        stateTimer = 10; // any value bigger than 1
        //player.anim.SetBool("SuccessfulCounterAttack", true);
    }
}
