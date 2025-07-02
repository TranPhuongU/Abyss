using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformState : PlayerState
{
    public PlayerTransformState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.stats.MakeInvincible(true);
        player.canDash = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.stats.MakeInvincible(false);
        if(!player.isForm2)
            player.isForm2 = true;
        else
            player.isForm2 = false;

        player.Transform();

        player.canDash = true;
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
