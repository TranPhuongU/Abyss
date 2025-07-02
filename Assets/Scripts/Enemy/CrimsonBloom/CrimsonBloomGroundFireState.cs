using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomGroundFireState : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    public CrimsonBloomGroundFireState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
            float chance = Random.value; // 0.0 – 1.0

            if (chance < 0.2f) // 0% → 35%
            {
                stateMachine.ChangeState(enemy.groundRushState);
            }
            else if (chance < 0.6f) // 35% → 70%
            {
                stateMachine.ChangeState(enemy.idleState);
            }
            else // 70% → 100%
            {
                stateMachine.ChangeState(enemy.groundTeleportExitState);
            }

        }
    }
}
