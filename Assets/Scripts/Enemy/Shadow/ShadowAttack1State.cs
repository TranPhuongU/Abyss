using UnityEngine;

public class ShadowAttack1State : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowAttack1State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        enemy.hitDetected = false;
    }

    public override void Update()
    {
        base.Update();

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
