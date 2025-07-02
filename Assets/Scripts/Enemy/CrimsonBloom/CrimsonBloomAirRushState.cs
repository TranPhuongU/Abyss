using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomAirRushState : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    private Vector3 direction;


    public CrimsonBloomAirRushState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.canFip = false;

        direction = (enemy.player.transform.position - enemy.transform.position).normalized;

        if (enemy.player.rb.linearVelocity.x != 0)
            direction = new Vector3(direction.x + .15f * enemy.player.facingDir, direction.y);


    }

    public override void Exit()
    {
        base.Exit();

        enemy.canFip = true;
    }

    public override void Update()
    {
        base.Update();

        //enemy.transform.right = rb.velocity;

        rb.linearVelocity = direction * enemy.speed;

        if (enemy.CanNotUltimate())
            stateMachine.ChangeState(enemy.idleState);

        if (enemy.CanUltimate() && enemy.player.dashing == false)
        {
            stateMachine.ChangeState(enemy.ultimateState);
        }

        
    }
}
