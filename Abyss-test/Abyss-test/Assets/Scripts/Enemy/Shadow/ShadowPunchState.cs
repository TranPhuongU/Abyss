using UnityEngine;
using static UnityEngine.Rendering.STP;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ShadowPunchState : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowPunchState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        enemy.SetZeroVelocity();

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

        if (!enemy.canFip)
            return;

        enemy.FlipController(enemy.moveDir);
    }
}
