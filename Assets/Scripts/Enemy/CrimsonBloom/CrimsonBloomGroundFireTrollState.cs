using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomGroundFireTrollState : EnemyState
{
    public Enemy_CrimsonBloom enemy;

    private bool playerDashed;

    public CrimsonBloomGroundFireTrollState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();

        stateTimer = .1f;

        playerDashed = false;
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

        if (enemy.player.dashing)
            playerDashed = true;

        if (triggerCalled)
        {
            if (playerDashed)
                stateMachine.ChangeState(enemy.trollState);
            else
                stateMachine.ChangeState(enemy.idleState);
        }
    }
}
