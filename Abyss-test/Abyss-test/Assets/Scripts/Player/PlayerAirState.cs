using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float timeToSmoke = .5f;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = timeToSmoke;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected() && !player.canClimb)
            stateMachine.ChangeState(player.wallSlide);

        if (player.IsGroundDetected())
        {
            if (stateTimer <= 0)
            {
                player.CreateLandingSmoke();
                AudioManager.instance.PlaySFX(41, null);

            }
            stateMachine.ChangeState(player.idleState);
        }

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.linearVelocity.y);
    }
}
