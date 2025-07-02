using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomShieldExplosionState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public CrimsonBloomShieldExplosionState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            float chance = Random.value; // Trả về số thực từ 0.0 đến 1.0

            if (chance < 0.5f) // 30% cơ hội vào trạng thái trên không
            {
                stateMachine.ChangeState(enemy.idleState);
            }
            else // 70% còn lại là dưới đất
            {
                stateMachine.ChangeState(enemy.groundTeleportExitState);
            }
        }
    }
}
