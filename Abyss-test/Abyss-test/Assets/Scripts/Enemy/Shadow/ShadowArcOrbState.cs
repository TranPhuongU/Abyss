using UnityEngine;

public class ShadowArcOrbState : EnemyState
{
    Enemy_Shadow enemy;

    private int arcOrbAmount;

    public ShadowArcOrbState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX2(49, enemy.transform);

        arcOrbAmount = enemy.arcOrbAmount;

        enemy.stats.magicResistance.AddModifier(enemy.buffArmorPercent);
        enemy.stats.armor.AddModifier(enemy.buffArmorPercent);

        stateTimer = .03f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.magicResistance.RemoveModifier(enemy.buffArmorPercent);
        enemy.stats.armor.RemoveModifier(enemy.buffArmorPercent);

    }

    public override void Update()
    {
        base.Update();

        if(stateTimer <= 0)
        {
            enemy.ArcOrbAttackTrigger();

            arcOrbAmount--;
            stateTimer = .03f;

            if(arcOrbAmount <= 0)
            {
                 stateMachine.ChangeState(enemy.chooseSkillState);
            }

        }
    }
}
