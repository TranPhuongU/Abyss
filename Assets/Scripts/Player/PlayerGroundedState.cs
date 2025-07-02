using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && player.skill.silentWorld.silentWorldUnlocked && player.isForm2 && player.canDash)
        {   
            if(player.skill.silentWorld.cooldownTimer > 0)
            {
               // player.fx.CreatePopUpText("Cooldown!", Color.magenta);
                return;
            }
            stateMachine.ChangeState(player.blackHole);
        }

        if (Input.GetKeyDown(KeyCode.E) && HasNoSword() && player.skill.sword.swordUnlocked && player.isForm2 && player.skill.sword.CanUseSkill())
        {
            stateMachine.ChangeState(player.aimSowrd);
        }
            

        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked && player.skill.parry.CanUseSkill())
            stateMachine.ChangeState(player.counterAttack);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        if (Input.GetKeyDown(KeyCode.Tab) && player.skill.morph.CanUseSkill() && player.skill.morph.morph)
            stateMachine.ChangeState(player.transformState);
    }
    
    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        //player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
    
}
