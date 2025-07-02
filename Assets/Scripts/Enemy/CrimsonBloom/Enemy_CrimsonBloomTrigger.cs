using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Behavior;

public class Enemy_CrimsonBloomTrigger : Enemy_AnimationTrigger
{

    //[SerializeField] private BehaviorGraphAgent behaviorAgent;
    //[SerializeField] private string blackboardVariableName = "animationTrigger";


    public GameObject explosionPrefab;

    private Enemy_CrimsonBloom enemyCrimson;

    protected override void Start()
    {
        base.Start();

        //// Tự động tìm BehaviorGraphAgent nếu chưa assign
        //if (behaviorAgent == null)
        //{
        //    behaviorAgent = GetComponentInParent<BehaviorGraphAgent>();
        //}

        enemyCrimson = GetComponentInParent<Enemy_CrimsonBloom>();

    }
    protected override void AttackTriggerMagic()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {

                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoMagicalDamage(target);
            }
        }
    }

    public void Explosion1Trigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyCrimson.explosionPoint.position, enemyCrimson.explosionRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {

                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoMagicalDamage(target);
            }

        }
    }

    public void Explosion2Trigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyCrimson.explosionPoint.position, enemyCrimson.explosion2Radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {

                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoMagicalDamage(target);
            }

        }
    }

    private void UltimateTrigger()
    {
        enemyCrimson.player.stats.currentHealth -= Mathf.RoundToInt(enemy.player.stats.GetMaxHealthValue() * .15f);
    }

    protected override void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {

                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
                enemyCrimson.hitDetected = true;
            }
        }
    }

    private void AnimationSpecialKameTrigger() => enemyCrimson.AnimationSpecialKameTrigger();
    private void AnimationSpecialGroundFireTrigger() => enemyCrimson.AnimationSpecialGroundFireTrigger();

    private void HidePlayer() => enemyCrimson.player.HidePlayerForCinematic();

    private void ShowPlayer() => enemyCrimson.player.ShowPlayerForCinematic();

    public void StartExplosionTowardsPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 50f, enemy.whatIsGround);
        Vector3 startPos = hit.point;

        GameObject newExplosion = Instantiate(explosionPrefab, startPos, Quaternion.identity);

        newExplosion.GetComponent<CrimsonLazeExplosion_Controller>().SetupExplosion(enemy.stats,1, enemy.facingDir, enemy);
    }

    // Method này sẽ được gọi từ Animation Event
    //public void OnAnimationComplete()
    //{
    //    behaviorAgent.BlackboardReference.SetVariableValue(blackboardVariableName, true);
    //}

    private void LightingSkill() => enemyCrimson.StartCoroutineLightScreen();



}

