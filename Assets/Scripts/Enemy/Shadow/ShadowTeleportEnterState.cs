using UnityEngine;

public class ShadowTeleportEnterState : EnemyState
{
    Enemy_Shadow enemy;
    public ShadowTeleportEnterState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX2(47, enemy.transform);

        if (enemy.playerOnPlatform)
        {
            enemy.transform.position = enemy.player.transform.position;
        }
        else
            enemy.FindPosition(); // dịch chuyển tới vị trí mới

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
            stateMachine.ChangeState(enemy.chooseSkillState); 
        }
    }

}
