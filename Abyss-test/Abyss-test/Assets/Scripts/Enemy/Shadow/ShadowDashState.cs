using UnityEngine;

public class ShadowDashState :EnemyState
{
    Enemy_Shadow enemy;
    public ShadowDashState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.stats.MakeInvincible(true);
        
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.MakeInvincible(false);

        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.dashSpeed * enemy.facingDir, 0);

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.fireState);
        }
    }

}
