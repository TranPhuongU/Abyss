using UnityEngine;

public class ShadowMoveState : EnemyState
{
    Enemy_Shadow enemy;
    private int moveDir;

    private bool hasCheckedDodge = false;

    public ShadowMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // Boss di chuyển trong thời gian ngẫu nhiên
        stateTimer = Random.Range(1f, 2f);

        if (enemy.player.transform.position.x > enemy.transform.position.x)
            moveDir = 1;
        else
            moveDir = -1;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        // Di chuyển liên tục trong thời gian còn lại
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.linearVelocity.y);


        if (enemy.IsPlayerDetected().distance <= enemy.attackDistance)
        {
            stateMachine.ChangeState(enemy.chooseSkillState);
        }

        // Nếu hết thời gian thì cũng chọn kỹ năng
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.chooseSkillState);
        }

        CheckDashState();
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
