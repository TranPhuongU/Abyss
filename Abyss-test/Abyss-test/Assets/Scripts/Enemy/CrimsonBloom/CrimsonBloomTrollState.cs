using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomTrollState : EnemyState
{
    public Enemy_CrimsonBloom enemy;

    private bool playerWasNear = false;
    public CrimsonBloomTrollState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.canFip = false;

        stateTimer = 2;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.canFip = true;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.groundTeleportExitState);
        }

        bool playerIsNear = Vector2.Distance(enemy.transform.position, enemy.player.transform.position) < .7f;

        if (playerIsNear && !playerWasNear)
        {
            // Chỉ chạy khi player mới vào vùng gần
            if (enemy.player.dashing)
            {
                if (Random.Range(0, 100) < 70)
                    stateMachine.ChangeState(enemy.dodgeState);
            }
            else
            {
                if (Random.Range(0, 100) < 50)
                    stateMachine.ChangeState(enemy.dodgeState);
            }
        }

        playerWasNear = playerIsNear;
    }
}
