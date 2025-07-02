using UnityEngine;

public class ShadowAttack2State : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowAttack2State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        ShadowFlipController();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.chooseSkillState);
    }

    private void ShadowFlipController()
    {
        if (enemy.player.transform.position.x > enemy.transform.position.x)
            enemy.moveDir = 1;
        else if (enemy.player.transform.position.x < enemy.transform.position.x)
            enemy.moveDir = -1;

        enemy.FlipController(enemy.moveDir);
    }
}
