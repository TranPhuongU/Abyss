using UnityEngine;

public class ShadowArcOrbTeleportEnterState : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowArcOrbTeleportEnterState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.transform.position = enemy.arcOrbTeleport.position;
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
            stateMachine.ChangeState(enemy.arcOrbState); 
        }
    }
}
