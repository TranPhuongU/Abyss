using UnityEngine;

public class ShadowIdleState : EnemyState
{
    Enemy_Shadow enemy;

    private bool hasCheckedDodge = false;


    public ShadowIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = Random.Range(.5f, 1.5f);

    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if(!enemy.fighting)
            return;

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }

        CheckDashState();


        //if (stateTimer < 0 && enemy.bossFightBegin)
        //    stateMachine.ChangeState(enemy.moveState);
    }

    private void CheckDashState()
    {
        if (hasCheckedDodge)
            return; // đã check né rồi thì bỏ qua

        float dist = Vector2.Distance(enemy.transform.position, enemy.player.transform.position);
        bool playerInMelee = dist <= enemy.checkPlayerRadius;
        bool playerIsDashing = enemy.player.dashing;

        if (playerInMelee)
        {
            hasCheckedDodge = true;

            float dodgeChance = playerIsDashing ? 0.3f : 0.2f;

            if (Random.value < dodgeChance)
            {
                stateMachine.ChangeState(enemy.dashState); // boss né tránh
            }
        }
    }
}
