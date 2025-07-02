using UnityEngine;
using System.Collections.Generic;
public class CBAirChooseSkillState : EnemyState
{
    Enemy_CrimsonBloom enemy;

    public CBAirChooseSkillState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _anim, Enemy_CrimsonBloom _enemy)
        : base(_enemyBase, _stateMachine, _anim)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.01f;
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(GetAirSkill());
    }

    private EnemyState GetAirSkill()
    {
        List<(EnemyState state, float weight)> options = new()
        {
            (enemy.airLazeState,          2f),
            (enemy.airTeleportEnterState, 5f),
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
