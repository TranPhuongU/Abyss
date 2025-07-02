using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CrimsonBloomGroundTeleportEnterState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public float xOffset;
    public CrimsonBloomGroundTeleportEnterState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }



    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = enemy.defaultGravity;

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
            stateMachine.ChangeState(enemy.attack1State);
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
                    xOffset = enemy.player.facingDir * 2.5f;
                else
                    xOffset = -enemy.player.facingDir * 2.5f; // đổi hướng
            }
            else
            {
                // Ngẫu nhiên trái/phải nếu người chơi đứng yên
                xOffset = Random.Range(0, 100) < 50 ? 1.6f : -1.6f;
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

        Debug.LogWarning("Boss không tìm được vị trí hợp lệ sau nhiều lần thử.");
    }
}
