using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    private Enemy_Shady enemy;

    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
     
    public override void Enter()
    {
        base.Enter();
        PlayerManager.instance.player.moveSpeed *= 0.6f;

        AudioManager.instance.PlaySFX2(50, enemy.transform);

        enemy.ShakeCamera();

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
            PlayerManager.instance.player.moveSpeed = PlayerManager.instance.player.defaultMoveSpeed;
            enemy.SelfDestroy();
        }
    }
}
