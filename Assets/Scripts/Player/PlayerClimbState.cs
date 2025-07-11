﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerState
{
    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.transform.position = player.climbBeginPosition;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public override void Exit()
    {
        base.Exit();

        player.LedgeClimbOver();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
