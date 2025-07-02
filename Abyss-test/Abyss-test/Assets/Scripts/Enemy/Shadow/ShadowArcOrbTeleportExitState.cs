using UnityEngine;

public class ShadowArcOrbTeleportExitState : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowArcOrbTeleportExitState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
            stateMachine.ChangeState(enemy.arbOrbTEnterState);
    }
}
