﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirDashState : PlayerState
{
    public PlayerAirDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //player.skill.dash.CloneOnDash();

        player.dashing = true;

        stateTimer = player.dashDuration;
        player.stats.MakeInvincible(true);

        player.consecutiveDashCount++;
        player.dashResetTimer = 0f; // Reset lại timer mỗi khi dash

        player.canDash = false;


    }

    public override void Exit()
    {
        base.Exit();

        //player.skill.dash.CloneOnArrival();
        player.SetVelocity(0, rb.linearVelocity.y);

        player.stats.MakeInvincible(false);

        player.canDash = true;

        player.dashing = false;

    }

    public override void Update()
    {
        base.Update();

        if (player.isForm2)
            player.skill.dash.CreateBomb();

        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer <= 0)
            stateMachine.ChangeState(player.idleState);

        player.fx.CreateAfterImage();
    }
}
