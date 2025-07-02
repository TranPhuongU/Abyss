using System.Collections;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.U2D.IK;
using UnityEngine.UIElements;

public class Enemy_CrimsonBloom : Enemy
{

    public CinemachineImpulseSource impulseSource;
    //public BehaviorGraphAgent behaviorAgent;

    [SerializeField] private Vector2 surroundingCheckSize;
    [SerializeField] private BoxCollider2D attackArea;

    public float checkPlayerRadius;
    public float checkGroundRadius;
    public float checkWallRadius;
    public float explosionRadius;
    public float explosion2Radius;

    public float speed = 10;
    public Vector2 jumpForce;

    public int randomSkill;

    public Transform airFirePoint;
    public Transform firePointDarkOrb;
    public Transform explosionPoint;

    public GameObject explosionPrefab;

    [Header("Air Fire detail")]

    public int moveDir;
    public float defaultGravity;

    private CrimsonBloom_Skill skill;
    #region Stats
    public CrimsonBloomIdleState idleState { get; private set; }
    public CrimsonBloomAttack1State attack1State { get; private set; }
    public CrimsonBloomAttack2State attack2State { get; private set; }
    public CrimsonBloomAttack3State attack3State { get; private set; }
    public CrimsonBloomAttack4State attack4State { get; private set; }
    public CrimsonBloomAttack5State attack5State { get; private set; }
    public CrimsonBloomAttack6State attack6State { get; private set; }
    public CrimsonBloomGroundTeleportExitState groundTeleportExitState { get; private set; }
    public CrimsonBloomGroundTeleportEnterState groundTeleportEnterState { get; private set; }
    public CrimsonBloomAirTeleportExitState airTeleportExitState { get; private set; }
    public CrimsonBloomAirTeleportEnterState airTeleportEnterState { get; private set; }
    public CrimsonBloomAirFireState airFireState { get; private set; }
    public CrimsonBloomLightingState lightingState { get; private set; }
    public CrimsonBloomHealingState healingState { get; private set; }
    public CrimsonBloomAirRushState airRushState { get; private set; }
    public CrimsonBloomGroundRushState groundRushState { get; private set; }
    public CrimsonBloomUltimateState ultimateState { get; private set; }
    public CrimsonBloomDodgeState dodgeState { get; private set; }
    public CrimsonBloomKameState kameState { get; private set; }
    public CrimsonBloomExplosion1State explosion1State { get; private set; }
    public CrimsonBloomExplosion2State explosion2State { get; private set; }
    public CrimsonBloomExplosion3State explosion3State { get; private set; }
    public CrimsonBloomExplosionTeleportEnterState explosionTeleportEnterState { get; private set; }
    public CrimsonBloomGroundFireTeleportEnterState groundFireTeleportEnterState { get; private set; }
    public CrimsonBloomGroundFireState groundFireState { get; private set; }
    public CrimsonBloomStunningState stunningState { get; private set; }
    public CrimsonBloomStunnedState stunnedState { get; private set; }
    public CrimsonBloomShieldState shieldState { get; private set; }
    public CrimsonBloomShieldExplosionState shieldExplosionState { get; private set; }
    public CrimsonBloomAirLazeState airLazeState { get; private set; }
    public CrimsonBloomJumpState jumpState { get; private set; }
    public CrimsonBloomTrollState trollState { get; private set; }
    public CrimsonBloomJumpAttackState jumpAttackState { get; private set; }
    public CrimsonBloomGroundFireTrollState groundFireTrollState { get; private set; }
    public CBAirChooseSkillState cBAirChooseSkillState { get; private set; }
    public CBGroundChooseSkillState cBGroundChooseSkillState { get; private set; }


    #endregion

    protected override void Awake()
    {
        base.Awake();

        skill = GetComponent<CrimsonBloom_Skill>();

        idleState = new CrimsonBloomIdleState(this, stateMachine, "Idle", this);
        jumpState = new CrimsonBloomJumpState(this, stateMachine, "Jump", this);
        trollState = new CrimsonBloomTrollState(this, stateMachine, "Troll", this);
        attack1State = new CrimsonBloomAttack1State(this, stateMachine, "Attack1", this);
        attack2State = new CrimsonBloomAttack2State(this, stateMachine, "Attack2", this);
        attack3State = new CrimsonBloomAttack3State(this, stateMachine, "Attack3", this);
        attack4State = new CrimsonBloomAttack4State(this, stateMachine, "Attack4", this);
        attack5State = new CrimsonBloomAttack5State(this, stateMachine, "Attack5", this);
        attack6State = new CrimsonBloomAttack6State(this, stateMachine, "Attack6", this);
        groundTeleportExitState = new CrimsonBloomGroundTeleportExitState(this, stateMachine, "GTeleportExit", this);
        groundTeleportEnterState = new CrimsonBloomGroundTeleportEnterState(this, stateMachine, "GTeleportEnter", this);
        airTeleportExitState = new CrimsonBloomAirTeleportExitState(this, stateMachine, "ATeleportExit", this);
        airTeleportEnterState = new CrimsonBloomAirTeleportEnterState(this, stateMachine, "ATeleportEnter", this);
        airFireState = new CrimsonBloomAirFireState(this, stateMachine, "AirFire", this);
        lightingState = new CrimsonBloomLightingState(this, stateMachine, "Lighting", this);
        healingState = new CrimsonBloomHealingState(this, stateMachine, "Healing", this);
        airRushState = new CrimsonBloomAirRushState(this, stateMachine, "Rush", this);
        groundRushState = new CrimsonBloomGroundRushState(this, stateMachine, "GroundRush", this);
        ultimateState = new CrimsonBloomUltimateState(this, stateMachine, "Ultimate", this);
        dodgeState = new CrimsonBloomDodgeState(this, stateMachine, "Dodge", this);
        kameState = new CrimsonBloomKameState(this, stateMachine, "Kame", this);
        explosion1State = new CrimsonBloomExplosion1State(this, stateMachine, "Explosion1", this);
        explosion2State = new CrimsonBloomExplosion2State(this, stateMachine, "Explosion2", this);
        explosion3State = new CrimsonBloomExplosion3State(this, stateMachine, "Explosion3", this);
        explosionTeleportEnterState = new CrimsonBloomExplosionTeleportEnterState(this, stateMachine, "GTeleportEnter", this);
        groundFireTeleportEnterState = new CrimsonBloomGroundFireTeleportEnterState(this, stateMachine, "GTeleportEnter", this);
        groundFireState = new CrimsonBloomGroundFireState(this, stateMachine, "GroundFire", this);
        stunningState = new CrimsonBloomStunningState(this, stateMachine, "Stunning", this);
        stunnedState = new CrimsonBloomStunnedState(this, stateMachine, "Stunned", this);
        shieldState = new CrimsonBloomShieldState(this, stateMachine, "Shield", this);
        shieldExplosionState = new CrimsonBloomShieldExplosionState(this, stateMachine, "ShieldExplosion", this);
        airLazeState = new CrimsonBloomAirLazeState(this, stateMachine, "AirLaze", this);
        jumpAttackState = new CrimsonBloomJumpAttackState(this, stateMachine, "JumpAttack", this);
        groundFireTrollState = new CrimsonBloomGroundFireTrollState(this, stateMachine, "GroundFireTroll", this);
        cBAirChooseSkillState = new CBAirChooseSkillState(this, stateMachine, "Idle", this);
        cBGroundChooseSkillState = new CBGroundChooseSkillState(this, stateMachine, "Idle", this);

    }
    protected override void Start()
    {
        base.Start();

        impulseSource = GetComponent<CinemachineImpulseSource>();

        defaultGravity = rb.gravityScale;

        stateMachine.Initialize(idleState);

        canFip = true;
    }

    protected override void Update()
    {
        base.Update();

        CrimsonFlipController();

        if (((EnemyStats)stats).currentShield <= 0 && canShield)
        {
            stateMachine.ChangeState(shieldState);

            //behaviorAgent.BlackboardReference.SetVariableValue("Shield", true);
        }
  
    }

    private void CrimsonFlipController()
    {
        if (player.transform.position.x > transform.position.x)
            moveDir = 1;
        else if (player.transform.position.x < transform.position.x)
            moveDir = -1;

        if (!canFip)
            return;

        FlipController(moveDir);
    }

    public void StartCoroutineLightScreen() => StartCoroutine(BlinkLight(0.2f, 30));

    public IEnumerator BlinkLight(float interval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            skill.flashScreen.SetActive(!skill.flashScreen.activeSelf);
            yield return new WaitForSeconds(interval);
        }

        skill.flashScreen.SetActive(false); // Tắt hẳn sau khi nhấp nháy xong
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newCrimsonBullet1 = Instantiate(skill.crimsonBullet1Prefab, airFirePoint.position, Quaternion.identity);

        Vector3 playerVelocity = player.rb.linearVelocity;
        float timeToHit = Vector2.Distance(airFirePoint.position, player.transform.position) / skill.starSpeed;
        Vector3 predictedPosition = player.transform.position + playerVelocity * timeToHit;
        Vector3 direction = (predictedPosition - airFirePoint.position).normalized;


        newCrimsonBullet1.GetComponent<CrimsonBullet1_Controller>().SetupCrimsonBullet(skill.starSpeed, stats, direction, skill.burnRadius, whatIsPlayer, skill.burnDelay, skill.firePrefab, skill.burnDamagePercent, skill.bulletDamagePercent, false);
    }
    public void AnimationSpecialKameTrigger()
    {
        GameObject newDarkOrb = Instantiate(skill.darkOrbPrefab, firePointDarkOrb.position, Quaternion.identity);

        newDarkOrb.GetComponent<DarkOrb_Controller>().SetupDarkOrb(skill.darkOrbSpeed * facingDir, stats, skill.damage, skill.damage);
        
    }

    public void AnimationSpecialGroundFireTrigger()
    {
        GameObject newCrimsonBullet1 = Instantiate(skill.crimsonBullet1Prefab,firePointDarkOrb.position, Quaternion.identity);

        Vector3 direction = Vector2.right * facingDir;

        newCrimsonBullet1.GetComponent<CrimsonBullet1_Controller>().SetupCrimsonBullet(skill.starSpeed, stats, direction, skill.burnRadius, whatIsPlayer, skill.burnDelay, skill.firePrefab, skill.burnDamagePercent, skill.bulletDamagePercent, true);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunningState);
            //behaviorAgent.BlackboardReference.SetVariableValue("isCountered", true);
            return true;
        }
        return false;
    }

    public bool CanUltimate() => Physics2D.OverlapCircle(transform.position, checkPlayerRadius, whatIsPlayer);
    public bool CanNotUltimate() => Physics2D.OverlapCircle(transform.position, checkGroundRadius, whatIsGround);
    
    public bool Stunned() => Physics2D.OverlapCircle(transform.position, checkWallRadius, whatIsGround);

    public RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    public bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    public bool PlayerInAttackArea()
    {
        return attackArea.bounds.Contains(player.transform.position);
    }

    public void ShakeCamera()
    {
        impulseSource.GenerateImpulse(); // ✅ đúng

    }

    public GameObject GetPlayerGameObject()
    {
        return player?.gameObject;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, checkPlayerRadius);
        Gizmos.DrawWireSphere(transform.position, checkGroundRadius);
        Gizmos.DrawWireSphere(transform.position, checkWallRadius);
        Gizmos.DrawWireSphere(explosionPoint.position, explosionRadius);
        Gizmos.DrawWireSphere(explosionPoint.position, explosion2Radius);
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);

    }

}
