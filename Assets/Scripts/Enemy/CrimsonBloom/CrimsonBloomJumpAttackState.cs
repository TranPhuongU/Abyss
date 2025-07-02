using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomJumpAttackState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public CrimsonBloomJumpAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.airTeleportExitState);
        }
    }
}
