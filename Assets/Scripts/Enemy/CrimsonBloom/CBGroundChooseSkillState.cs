using System.Collections.Generic;
using UnityEngine;
public class CBGroundChooseSkillState : EnemyState
{
    Enemy_CrimsonBloom enemy;

    public CBGroundChooseSkillState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _anim, Enemy_CrimsonBloom _enemy)
        : base(_enemyBase, _stateMachine, _anim)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = .01f;
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(GetGroundSkill());
    }

    private EnemyState GetGroundSkill()
    {
        List<(EnemyState state, float weight)> options = new()
        {
            (enemy.explosion1State,                 2f),
            (enemy.groundTeleportEnterState,        3f),
            (enemy.explosionTeleportEnterState,     3f),
            (enemy.groundFireTeleportEnterState,    2f),
        };

        return WeightedRandom(options);
    }

    private EnemyState WeightedRandom(List<(EnemyState state, float weight)> options)
    {
        float total = 0;
        foreach (var o in options) total += o.weight;

        float r = Random.value * total;
        foreach (var o in options)
        {
            if (r < o.weight) return o.state;
            r -= o.weight;
        }

        return options[0].state;
    }
}
