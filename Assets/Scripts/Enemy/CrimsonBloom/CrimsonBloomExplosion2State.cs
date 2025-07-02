using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomExplosion2State : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    public CrimsonBloomExplosion2State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .2f;

    }

    public override void Exit()
    {
        base.Exit();

        enemy.canFip = true;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            enemy.canFip = false;
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.groundTeleportExitState);
        }
    }
}
