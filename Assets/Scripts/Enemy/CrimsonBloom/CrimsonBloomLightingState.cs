using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomLightingState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public CrimsonBloomLightingState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        enemy.StartCoroutineLightScreen();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(enemy.groundTeleportExitState);
    }
}
