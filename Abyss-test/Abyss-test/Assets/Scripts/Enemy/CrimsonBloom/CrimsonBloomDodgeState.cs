using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomDodgeState : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    public CrimsonBloomDodgeState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        rb.linearVelocity = new Vector2(-enemy.facingDir * 20, 0);

        if (triggerCalled)
            stateMachine.ChangeState(enemy.kameState);
    }
}
