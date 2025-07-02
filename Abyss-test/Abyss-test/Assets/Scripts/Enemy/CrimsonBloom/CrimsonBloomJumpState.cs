using UnityEngine;

public class CrimsonBloomJumpState : EnemyState
{
    public Enemy_CrimsonBloom enemy;

    private Vector3 direction;
    public CrimsonBloomJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.canFip = false;

        stateTimer = .4f;

        rb.gravityScale = enemy.defaultGravity;

        rb.linearVelocity = new Vector2(enemy.jumpForce.x * enemy.facingDir, enemy.jumpForce.y);
    }

    public override void Exit()
    {
        base.Exit();
        
        enemy.canFip = true;
    }

    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat("yVelocity", rb.linearVelocity.y);

        if (enemy.CanUltimate())
            stateMachine.ChangeState(enemy.jumpAttackState);
        else
        {
            if (enemy.CanNotUltimate() && stateTimer <= 0)
                stateMachine.ChangeState(enemy.idleState);
        }
    }

}
