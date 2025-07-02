using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomStunningState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public CrimsonBloomStunningState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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

        rb.linearVelocity = Vector2.right * -enemy.facingDir * 25;

        if (enemy.Stunned())
        {
            stateMachine.ChangeState(enemy.stunnedState);
        }

    }
}
