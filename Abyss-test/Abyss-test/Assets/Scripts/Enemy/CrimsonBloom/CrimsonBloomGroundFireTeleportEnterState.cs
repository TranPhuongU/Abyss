
using UnityEngine;

public class CrimsonBloomGroundFireTeleportEnterState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public float xOffset;
    public CrimsonBloomGroundFireTeleportEnterState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = enemy.defaultGravity;

        if (Random.Range(0, 100) < 50)
            enemy.randomSkill = 0;
        else
        {
            enemy.randomSkill = 1;
        }

        FindPosition();
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

            float chance = Random.value; // 0.0 – 1.0

            if (chance < 0.2f) // 0% → 35%
            {
                stateMachine.ChangeState(enemy.groundFireTrollState);
            }
            else if (chance < 0.6f) // 35% → 70%
            {
                stateMachine.ChangeState(enemy.groundFireState);
            }
            else // 70% → 100%
            {
                stateMachine.ChangeState(enemy.kameState);
            }


        }
    }

    public void FindPosition()
    {
        const int maxAttempts = 10; // Giới hạn số lần thử
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            float xOffset;

            // Ưu tiên chọn hướng dựa trên tốc độ và hướng người chơi
            if (enemy.player.rb.linearVelocity.x != 0)
            {
                if (!enemy.SomethingIsAround())
                    xOffset = enemy.player.facingDir * 8f;
                else
                    xOffset = -enemy.player.facingDir * 8f; // đổi hướng
            }
            else
            {
                // Ngẫu nhiên trái/phải nếu người chơi đứng yên
                xOffset = Random.Range(0, 100) < 50 ? 6f : -6f;
            }

            // Thử đặt vị trí ở độ cao tạm thời
            Vector3 testPos = new Vector3(enemy.player.transform.position.x + xOffset, enemy.player.transform.position.y + 3);
            enemy.transform.position = testPos;

            RaycastHit2D hit = enemy.GroundBelow();
            if (hit)
            {
                // Canh đúng mặt đất
                float groundY = testPos.y - hit.distance + (enemy.cd.size.y / 2f);
                enemy.transform.position = new Vector3(testPos.x, groundY);

                // Nếu không có vật cản thì hoàn tất
                if (!enemy.SomethingIsAround())
                    return;
            }

            attempts++;
        }
    }

}
