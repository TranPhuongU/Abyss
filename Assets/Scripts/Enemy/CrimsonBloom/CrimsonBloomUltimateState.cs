using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomUltimateState : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    public CrimsonBloomUltimateState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.player.isForm2)
        {
            enemy.player.isForm2 = false;
            enemy.player.Transform();
        }

        enemy.stats.MakeInvincible(true);

        enemy.player.canDash = false;

        rb.gravityScale = 0;

        float xOffset = 1.45f * -enemy.facingDir;
       
        if (enemy.facingDir == -1)
            enemy.transform.position = new Vector3(enemy.player.transform.position.x + xOffset, enemy.player.transform.position.y + 0.63f);
        else if(enemy.facingDir == 1)
            enemy.transform.position = new Vector3(enemy.player.transform.position.x + xOffset, enemy.player.transform.position.y + 0.63f);

    }

    public override void Exit()
    {
        base.Exit();

        enemy.player.canDash = true;

        enemy.stats.MakeInvincible(false);

    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        enemy.player.SetZeroVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
