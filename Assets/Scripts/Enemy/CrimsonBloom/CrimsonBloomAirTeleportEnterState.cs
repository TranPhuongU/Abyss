using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomAirTeleportEnterState : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    public CrimsonBloomAirTeleportEnterState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.transform.position = new Vector3(enemy.player.transform.position.x + 9, enemy.player.transform.position.y + 7);
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
            stateMachine.ChangeState(enemy.airFireState);
            
    }
}
