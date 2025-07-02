using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomGroundRushState : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    private Vector3 direction;

    public CrimsonBloomGroundRushState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.canFip = false;

        direction = (enemy.player.transform.position - enemy.transform.position).normalized;


    }

    public override void Exit()
    {
        base.Exit();
        enemy.canFip = true;
    }

    public override void Update()
    {
        base.Update();

        rb.linearVelocity = direction * enemy.speed;

        if (enemy.IsWallDetected())
            stateMachine.ChangeState(enemy.idleState);

        if (enemy.CanUltimate() && enemy.player.dashing == false)
        {
            stateMachine.ChangeState(enemy.ultimateState);
        }
    }
}
