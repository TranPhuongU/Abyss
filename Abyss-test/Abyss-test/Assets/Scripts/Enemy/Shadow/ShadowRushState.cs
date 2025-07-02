using UnityEngine;

public class ShadowRushState : EnemyState
{
    Enemy_Shadow enemy;

    public int rushDir;
    public ShadowRushState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        ShadowFlipController();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();



        enemy.SetVelocity(enemy.rushSpeed * rushDir, 0);


        if (enemy.IsWallDetected())
            stateMachine.ChangeState(enemy.chooseSkillState);

        if (enemy.CanUltimate() && enemy.player.dashing == false)
        {
            stateMachine.ChangeState(enemy.punchState);
        }
    }

    private void ShadowFlipController()
    {
        if (enemy.player.transform.position.x > enemy.transform.position.x)
            rushDir = 1;
        else if (enemy.player.transform.position.x < enemy.transform.position.x)
            rushDir = -1;
    }
}
