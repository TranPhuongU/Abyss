using UnityEngine;
using System.Collections.Generic;

public class ShadowChooseSkillState : EnemyState
{
    Enemy_Shadow enemy;

    public bool usedUltimateAt70 = false;
    public bool usedUltimateAt40 = false;
    public bool usedUltimateAt10 = false;


    public ShadowChooseSkillState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shadow _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.1f;
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(GetNextSkillState());
        }
    }

    private EnemyState GetNextSkillState()
    {

        if (enemy.playerOnPlatform && !enemy.hasTeleportedToPlatform)
        {
            enemy.hasTeleportedToPlatform = true; // Đánh dấu đã dịch chuyển
            return enemy.teleportExitState;
        }


        float currentHP = enemy.stats.currentHealth;
        float maxHP = enemy.stats.GetMaxHealthValue();
        float hpPercent = currentHP / maxHP;

        // Ưu tiên dùng ultimate tại các mốc máu cụ thể
        if (hpPercent <= 0.1f && !usedUltimateAt10)
        {
            usedUltimateAt10 = true;
            return enemy.ultimateState;
        }
        else if (hpPercent <= 0.4f && !usedUltimateAt40)
        {
            usedUltimateAt40 = true;
            return enemy.ultimateState;
        }
        else if (hpPercent <= 0.7f && !usedUltimateAt70)
        {
            usedUltimateAt70 = true;
            return enemy.ultimateState;
        }

        if (enemy.PlayerInMeleeRange())
        {
            // Nếu trong tầm gần: 90% dùng cận chiến, 10% dùng kỹ năng tầm xa
            if (Random.value < 0.2f)
                return GetRangedSkill();

            return GetMeleeSkill();
        }
        else
        {
            // Nếu ở xa: chỉ dùng kỹ năng tầm xa
            return GetRangedSkill();
        }
    }

    private EnemyState GetMeleeSkill()
    {
        List<(EnemyState state, float weight)> options = new List<(EnemyState, float)>
        {
            (enemy.attack1State,      3f),
            (enemy.attack3State,      2f),
            (enemy.attack4State,      2f),
            (enemy.idleState,         1f),
            (enemy.teleportExitState, 1.3f),
            (enemy.moveState,         1f)
        };

        return GetWeightedRandomState(options);
    }

    private EnemyState GetRangedSkill()
    {
        List<(EnemyState state, float weight)> options = new List<(EnemyState, float)>
        {
            (enemy.fireState,         3f),
            (enemy.ultimateState,     0.7f),
            (enemy.rushState,         2f),
            (enemy.arbOrbTExitState,  1.5f),
            (enemy.teleportExitState, 1f),
            (enemy.attack2State,      2f),
            (enemy.moveState,         2f), 
            (enemy.idleState,         1f)
        };

        return GetWeightedRandomState(options);
    }

    private EnemyState GetWeightedRandomState(List<(EnemyState state, float weight)> options)
    {
        float totalWeight = 0f;
        float reductionFactor = 0.8f; // ✅ Giảm 30% trọng số nếu vừa dùng skill này

        List<(EnemyState state, float adjustedWeight)> adjustedOptions = new List<(EnemyState, float)>();

        foreach (var option in options)
        {
            float adjustedWeight = option.weight;

            if (option.state == enemy.lastUsedSkill)
                adjustedWeight *= reductionFactor;

            adjustedOptions.Add((option.state, adjustedWeight));
            totalWeight += adjustedWeight;
        }

        float randomValue = Random.value * totalWeight;

        foreach (var option in adjustedOptions)
        {
            if (randomValue < option.adjustedWeight)
            {
                enemy.lastUsedSkill = option.state; // lưu lại skill vừa dùng
                return option.state;
            }

            randomValue -= option.adjustedWeight;
        }

        // Trường hợp fallback
        enemy.lastUsedSkill = adjustedOptions[0].state;
        return adjustedOptions[0].state;
    }
}
