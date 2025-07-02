using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    public int comboCounter { get; private set; }

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(39, null);


        xInput = 0;  // we need this to fix bug on attack direction

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        if (player.skill.slashWave.canSlashWave && player.skill.slashWave.amountOfSlashWave > 0 && !player.isForm2)
        {
            
            if (comboCounter == 0 && player.skill.dash.slashWave1)
                player.skill.slashWave.CreateSlashWave1(player.attackCheck);

            if (comboCounter == 1 && player.skill.dash.slashWave2)
                player.skill.slashWave.CreateSlashWave2(player.attackCheck);

            if (comboCounter == 2 && player.skill.dash.slashWave3)
                player.skill.slashWave.CreateSlashWave3(player.attackCheck);

            player.skill.slashWave.amountOfSlashWave--;
            player.skill.slashWave.canSlashWaveTimer = 1;
        }


        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;
        

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);


        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        AudioManager.instance.StopSFX(39);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    
}
