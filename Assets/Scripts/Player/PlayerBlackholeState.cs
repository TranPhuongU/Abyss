using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;


    private float defaultGravity;
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        //AudioManager.instance.PlaySFX(36, player.transform);

        player.canDash = false;

        player.stats.MakeInvincible(true);

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;

        player.darkScreenUltimate.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.canDash = true;

        player.rb.gravityScale = defaultGravity;

        player.darkScreenUltimate.SetActive(false);
        player.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.linearVelocity = new Vector2(0, 15);

        if (stateTimer < 0)
        {
            rb.linearVelocity = new Vector2(0, -.1f);

            if (!skillUsed)
            {
                if(player.skill.silentWorld.CanUseSkill())
                    skillUsed = true;
            }
        }

        if (player.skill.silentWorld.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
}
