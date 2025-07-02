using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonBloomExplosion1State : EnemyState
{
    private Enemy_CrimsonBloom enemy;
    private float xOffset;

    public CrimsonBloomExplosion1State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .2f;

        rb.gravityScale = enemy.defaultGravity;

        FindPosition();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.canFip = true;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0)
        {
            enemy.canFip = false;
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.idleState);
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
                    xOffset = enemy.player.facingDir * 3f;
                else
                    xOffset = -enemy.player.facingDir * 3f; // đổi hướng
            }
            else
            {
                // Ngẫu nhiên trái/phải nếu người chơi đứng yên
                xOffset = Random.Range(0, 100) < 50 ? 2.5f : -2.5f;
            }

            // Thử đặt vị trí ở độ cao tạm thời
            Vector3 testPos = new Vector3(enemy.player.transform.position.x + xOffset, enemy.player.transform.position.y +3);
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

        Debug.LogWarning("Boss không tìm được vị trí hợp lệ sau nhiều lần thử.");
    }

}
