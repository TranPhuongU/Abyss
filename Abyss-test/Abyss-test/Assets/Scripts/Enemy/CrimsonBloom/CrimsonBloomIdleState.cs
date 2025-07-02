using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CrimsonBloomIdleState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    private bool playerWasNear;

    public CrimsonBloomIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = enemy.defaultGravity;

        stateTimer = Random.Range(0.5f,1.5f);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if(!enemy.fighting)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            stateMachine.ChangeState(enemy.healingState);
        }

        if(stateTimer <= 0)
        {
            float chance = Random.value; // 0.0 – 1.0

            if (chance < 0.1f) // 0% → 35%
            {
                stateMachine.ChangeState(enemy.healingState);
            }
            else if (chance < 0.55f) // 35% → 70%
            {
                stateMachine.ChangeState(enemy.groundFireState);
            }
            else // 70% → 100%
            {
                stateMachine.ChangeState(enemy.groundTeleportExitState);
            }
        }

        bool playerIsNear = Vector2.Distance(enemy.transform.position, enemy.player.transform.position) < .7f;

        if (playerIsNear && !playerWasNear)
        {
            // Chỉ chạy khi player mới vào vùng gần
            if (enemy.player.dashing)
            {
                if (Random.Range(0, 100) < 25)
                    stateMachine.ChangeState(enemy.dodgeState);
            }
            else
            {
                if (Random.Range(0, 100) < 15)
                    stateMachine.ChangeState(enemy.dodgeState);
            }
        }

        playerWasNear = playerIsNear;

    }
}
