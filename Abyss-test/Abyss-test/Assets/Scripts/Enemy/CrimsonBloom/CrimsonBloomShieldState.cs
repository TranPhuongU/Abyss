using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomShieldState : EnemyState
{
    public Enemy_CrimsonBloom enemy;

    public float shieldTimer = 1f;
    public CrimsonBloomShieldState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.canFip = false;

        enemy.canShield = false;

        stateTimer = shieldTimer;

        enemy.stats.magicResistance.AddModifier(Mathf.RoundToInt(enemy.stats.magicResistance.GetValue() * .5f));
    }

    public override void Exit()
    {
        base.Exit();

        enemy.canShield = true;

        enemy.canFip = true;

        enemy.stats.magicResistance.RemoveModifier(Mathf.RoundToInt(enemy.stats.magicResistance.baseValue * .5f));
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0)
        {
            ((EnemyStats)enemy.stats).currentShield += Mathf.RoundToInt(((EnemyStats)enemy.stats).maxShield * .1f);

            if (((EnemyStats)enemy.stats).onShieldChanged != null)
                ((EnemyStats)enemy.stats).onShieldChanged();

            stateTimer = shieldTimer;
        }

        

        if (((EnemyStats)enemy.stats).currentShield >= ((EnemyStats)enemy.stats).maxShield)
        {
            stateMachine.ChangeState(enemy.shieldExplosionState);
        }


    }
}
