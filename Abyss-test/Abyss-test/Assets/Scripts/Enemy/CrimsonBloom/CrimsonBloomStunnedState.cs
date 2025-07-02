using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomStunnedState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public CrimsonBloomStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = 60;

        ((EnemyStats)enemy.stats).currentShield -= Mathf.RoundToInt(((EnemyStats)enemy.stats).maxShield * .25f);

        if (((EnemyStats)enemy.stats).onShieldChanged != null)
            ((EnemyStats)enemy.stats).onShieldChanged();
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = enemy.defaultGravity;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
