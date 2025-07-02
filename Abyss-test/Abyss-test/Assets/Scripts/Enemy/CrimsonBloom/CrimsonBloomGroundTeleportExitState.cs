using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomGroundTeleportExitState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public CrimsonBloomGroundTeleportExitState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.canFip = false;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.canFip = true;
    }

    public override void Update()
    {
        base.Update();


        if (triggerCalled)
        {
            float chance = Random.value; // Trả về số thực từ 0.0 đến 1.0

            if (chance < 0.3f) // 30% cơ hội vào trạng thái trên không
            {
                stateMachine.ChangeState(enemy.cBAirChooseSkillState);
            }
            else // 70% còn lại là dưới đất
            {
                stateMachine.ChangeState(enemy.cBGroundChooseSkillState);
            }

        }
    }
}
