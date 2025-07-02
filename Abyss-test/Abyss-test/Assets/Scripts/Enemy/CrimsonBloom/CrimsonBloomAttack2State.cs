using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomAttack2State : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public CrimsonBloomAttack2State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        enemy.hitDetected = false;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            // Nếu attack 1 hit được player, chuyển sang attack 2
            if (enemy.hitDetected)
            {
                stateMachine.ChangeState(enemy.attack3State);
            }
            else
            {
                // Nếu không hit, quay về idle hoặc state khác
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }
}
