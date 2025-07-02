using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicUltimateState : PlayerState
{
    public RaycastHit2D checkGround;
    public PlayerPhysicUltimateState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(44, null);

        player.canDash = false;
        
        player.stats.damage.AddModifier(player.skill.physicUltimate.damageIncrease);

        if (player.skill.physicUltimate.increaseCritRateUnlocked)
        {
            player.stats.critChance.AddModifier(100);
            player.stats.critPower.AddModifier(100);
        }
       
        player.stats.MakeInvincible(true);

        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Cần thay đổi để physics hoạt động đúng

        player.anim.updateMode = AnimatorUpdateMode.UnscaledTime;

        player.darkScreenUltimate.SetActive(true);

    }


    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(44);

        player.canDash = true;

        player.stats.damage.RemoveModifier(player.skill.physicUltimate.damageIncrease);
        
        player.stats.MakeInvincible(false);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        player.anim.updateMode = AnimatorUpdateMode.Normal;

        player.darkScreenUltimate.SetActive(false);
        player.EnterFireSwordMode();


        if (checkGround.collider == null)
        {
            // Không có tường, được phép dịch chuyển
            player.transform.position = new Vector3(player.transform.position.x + (2.3f * player.facingDir), player.transform.position.y);
        }
        else
        {
            // Có tường → chỉ dịch chuyển tới gần tường
            float safeDistance = checkGround.distance - 0.1f; // tránh chạm tường
            player.transform.position = new Vector3(player.transform.position.x + safeDistance * player.facingDir, player.transform.position.y);
        }

    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();

        checkGround = Physics2D.Raycast(player.transform.position, Vector2.right * player.facingDir, 2.4f, player.whatIsGround);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

}
