using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomAirLazeState : EnemyState
{
    private Enemy_CrimsonBloom enemy;

    public float xOffset;
    public CrimsonBloomAirLazeState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = 0;


        enemy.transform.position = new Vector3(enemy.player.transform.position.x + 8, enemy.player.transform.position.y + 7);
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
            float chance = Random.value; // 0.0 – 1.0

            if (chance < 0.3f) // 0% → 35%
            {
                stateMachine.ChangeState(enemy.airRushState);
            }
            else
            {
                stateMachine.ChangeState(enemy.airTeleportExitState);
            }
           
        }
    }
}
