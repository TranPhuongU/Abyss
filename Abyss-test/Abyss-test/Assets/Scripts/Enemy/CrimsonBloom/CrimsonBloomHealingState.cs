using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomHealingState : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    private float healthingTimer;
    private float shakeTimer;
    public CrimsonBloomHealingState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 5;

        healthingTimer = .3f;
        shakeTimer = 0.01f;

        enemy.canFip = false;

        enemy.randomSkill = Random.Range(0, 2);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.canFip = true;
    }

    public override void Update()
    {
        base.Update();

        shakeTimer -= Time.deltaTime;
        if (shakeTimer <= 0)
        {
            enemy.impulseSource.GenerateImpulse(); // hoặc truyền hướng
            shakeTimer = 0.3f; // nhịp rung
        }


        healthingTimer -= Time.deltaTime;

        if(healthingTimer <= 0)
        {
            if(enemy.stats.currentHealth == enemy.stats.GetMaxHealthValue())
                return;

            enemy.stats.currentHealth += Mathf.RoundToInt(enemy.stats.GetMaxHealthValue() * 0.01f);

            if (enemy.stats.onHealthChanged != null)
                enemy.stats.onHealthChanged();

            healthingTimer = .3f;
        }

        if (stateTimer <= 0)
        {
            if(enemy.randomSkill == 0)
                stateMachine.ChangeState(enemy.idleState);
            else if(enemy.randomSkill == 1)
                stateMachine.ChangeState(enemy.groundRushState);
        }
    }
}
