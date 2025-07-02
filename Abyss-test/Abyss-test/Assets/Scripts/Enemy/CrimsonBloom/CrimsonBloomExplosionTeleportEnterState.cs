using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrimsonBloomExplosionTeleportEnterState : EnemyState
{
    public Enemy_CrimsonBloom enemy;
    public float xOffset;
    public CrimsonBloomExplosionTeleportEnterState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_CrimsonBloom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.randomSkill = Random.Range(0, 2);

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
            if (enemy.randomSkill == 0)
                stateMachine.ChangeState(enemy.explosion2State);
            else if (enemy.randomSkill == 1)
                stateMachine.ChangeState(enemy.explosion3State);
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
