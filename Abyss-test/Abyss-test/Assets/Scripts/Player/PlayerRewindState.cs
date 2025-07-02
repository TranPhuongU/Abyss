using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerRewindState : PlayerState
{
    
    private float rewindTimer;
    // Lưu trạng thái ban đầu trước khi tua
    private float originalRotationY;
    private int originalFacingDir;
    private bool originalFacingRight;

    public PlayerRewindState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        player.canDash = false;

        player.stats.MakeInvincible(true);

        AudioManager.instance.PlaySFX(38, player.transform);

        rewindTimer = 0f;

        // Lưu hướng ban đầu
        originalRotationY = player.transform.eulerAngles.y;
        originalFacingDir = player.facingDir;
        originalFacingRight = player.facingRight;

        player.rb.simulated = false; // tắt vật lý khi tua
        player.anim.speed = 0;       // dừng animation
        player.skill.rewind.CreateClockKurumi();
    }

    public override void Exit()
    {
        base.Exit();

        player.canDash = true;

        player.rb.simulated = true;
        player.anim.speed = 1;

        // Khôi phục trạng thái ban đầu
        player.transform.eulerAngles = new Vector3(0, originalRotationY, 0);
        player.facingDir = originalFacingDir;
        player.facingRight = originalFacingRight;

        player.skill.rewind.CanUseSkill();

        player.stats.MakeInvincible(false);
        
    }

    public override void Update()
    {
        base.Update();

        rewindTimer += Time.deltaTime;

        float normalizedTime = rewindTimer / player.skill.rewind.rewindDuration;
        float targetTime = Time.time - normalizedTime * player.skill.rewind.recordTime;

        RewindData before = null, after = null;

        for (int i = player.skill.rewind.positionHistory.Count - 1; i > 0; i--)
        {
            if (player.skill.rewind.positionHistory[i].timestamp >= targetTime &&
                player.skill.rewind.positionHistory[i - 1].timestamp <= targetTime)
            {
                after = player.skill.rewind.positionHistory[i];
                before = player.skill.rewind.positionHistory[i - 1];
                break;
            }
        }

        if (before != null && after != null)
        {
            float t = Mathf.InverseLerp(before.timestamp, after.timestamp, targetTime);
            Vector3 pos = Vector3.Lerp(before.position, after.position, t);

            player.transform.position = pos;

            // Cập nhật hướng từ dữ liệu
            player.transform.eulerAngles = new Vector3(0, before.rotationY, 0);
            player.facingRight = before.rotationY == 0;
            player.facingDir = player.facingRight ? 1 : -1;

            player.stats.currentHealth = before.currentHealth;
            player.stats.onHealthChanged();
            

            player.anim.Play(before.animStateHash, 0, before.animNormalizedTime);
        }

        if (rewindTimer >= player.skill.rewind.rewindDuration)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
