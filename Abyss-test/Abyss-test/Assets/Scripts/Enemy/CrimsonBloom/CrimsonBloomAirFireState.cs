using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CrimsonBloomAirFireState : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    public CrimsonBloomAirFireState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.airTeleportExitState);
        }    
    }
}
