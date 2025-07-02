using System.Data;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class Enemy_Shadow : Enemy
{

    [SerializeField] private PlayableDirector playableDirector;

    public int randomSkill;
    public int moveDir;
    public EnemyState lastUsedSkill; // Lưu skill được chọn gần nhất
    public bool playerOnPlatform;
    public bool hasTeleportedToPlatform = false;
    public CinemachineImpulseSource impulseSource;

    [Header("Player Detect Distance")]
    [SerializeField] private float meleeRange = 3f; // có thể chỉnh trong Inspector

    public bool bossFightBegin;
    [Header("Light orb detail")]
    [SerializeField] private GameObject lightOrb;
    [SerializeField] private float bigDamagePercent;
    [SerializeField] private float minDamagePercent;
    [SerializeField] private float bigLightOrbSpeed;
    [SerializeField] private float minLightOrbSpeed;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform horizontalFirePoint;
    private float lightOrbSize;

    [Header("Boom detail")]
    public Transform boomPoint;
    public float boomRadius;
    public float boomDMGPercent;

    [Header("Arc orb detail")]
    [SerializeField] private float arcOrbSpeed;
    [SerializeField] private float arcOrbDMG;
    public int buffArmorPercent;
    public int arcOrbAmount;
    public Transform arcOrbTeleport;

    [Header("Rush detail")]
    public float checkPlayerRadius;
    public float rushSpeed;

    [Header("Ultimate detail")]
    [SerializeField] GameObject explosionPrefab;
    public float explosionOffset;
    public int explosionAmount;
    public int buffPercent;

    [Header("Dash detail")]
    public float dashSpeed;

    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;


    #region Stats
    public ShadowDeadState deadState {  get; private set; }
    public ShadowIdleState idleState {  get; private set; }
    public ShadowMoveState moveState {  get; private set; }
    public ShadowFireState fireState { get; private set; }
    public ShadowDashState dashState { get; private set; }
    public ShadowAttack1State attack1State { get; private set; }
    public ShadowAttack2State attack2State { get; private set; }
    public ShadowAttack3State attack3State { get; private set; }
    public ShadowAttack4State attack4State { get; private set; }
    public ShadowUltimateState ultimateState { get; private set; }
    public ShadowTeleportExitState teleportExitState { get; private set;}
    public ShadowTeleportEnterState teleportEnterState { get; private set ; }
    public ShadowPunchState punchState { get; private set; }
    public ShadowRushState rushState { get; private set; }
    public ShadowArcOrbState arcOrbState { get; private set; }
    public ShadowChooseSkillState chooseSkillState { get; private set; }
    public ShadowArcOrbTeleportExitState arbOrbTExitState {  get; private set; }
    public ShadowArcOrbTeleportEnterState arbOrbTEnterState {  get; private set; }
    

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadowIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadowMoveState(this, stateMachine, "Move", this);
        rushState = new ShadowRushState(this, stateMachine, "Rush", this);
        fireState = new ShadowFireState(this, stateMachine, "Fire", this);
        dashState = new ShadowDashState(this, stateMachine, "Dash", this);
        deadState = new ShadowDeadState(this, stateMachine, "TeleportExit", this);
        punchState = new ShadowPunchState(this, stateMachine, "Punch", this);
        arcOrbState = new ShadowArcOrbState(this, stateMachine, "ArcOrb", this);
        attack1State = new ShadowAttack1State(this, stateMachine, "Attack1", this);
        attack2State = new ShadowAttack2State(this, stateMachine, "Attack2", this);
        attack3State = new ShadowAttack3State(this, stateMachine, "Attack3", this);
        attack4State = new ShadowAttack4State(this, stateMachine, "Attack4", this);
        ultimateState = new ShadowUltimateState(this, stateMachine, "Ultimate", this);
        chooseSkillState = new ShadowChooseSkillState(this, stateMachine, "Idle", this);
        teleportExitState = new ShadowTeleportExitState(this, stateMachine, "TeleportExit", this);
        teleportEnterState = new ShadowTeleportEnterState(this, stateMachine, "TeleportEnter", this);
        arbOrbTExitState = new ShadowArcOrbTeleportExitState(this, stateMachine, "TeleportExit", this);
        arbOrbTEnterState = new ShadowArcOrbTeleportEnterState(this, stateMachine, "TeleportEnter", this);


    }

    protected override void Start()
    {
        base.Start();

        impulseSource = GetComponent<CinemachineImpulseSource>();


        stateMachine.Initialize(idleState);

        buffArmorPercent = Mathf.RoundToInt(stats.magicResistance.GetValue() * 1f);
        buffArmorPercent = Mathf.RoundToInt(stats.armor.GetValue() * 1f);

        buffPercent = Mathf.RoundToInt(stats.magicResistance.GetValue() * 2f);
        buffPercent = Mathf.RoundToInt(stats.armor.GetValue() * 2f);
    }

    protected override void Update()
    {
        base.Update();
    }


    public override void AnimationSpecialAttackTrigger()
    {
        Vector3 direction = (player.transform.position - firePoint.transform.position).normalized;

        if(transform.position.x <= player.transform.position.x)
        {
            if (player.rb.linearVelocity.x > 0.1f)        
                direction.x += .3f;
            else if (player.rb.linearVelocity.x < -0.1f) 
                direction.x -= .2f;
        }
        else if(transform.position.x > player.transform.position.x)
        {
            if (player.rb.linearVelocity.x > 0.1f)        
                direction.x += .2f;
            else if (player.rb.linearVelocity.x < -0.1f)  
                direction.x -= .3f;
        }

        direction = direction.normalized;


        GameObject newLightOrb = Instantiate(lightOrb, firePoint.position, Quaternion.identity);

        newLightOrb.GetComponent<LightOrb_Controller>().SetupLightOrb(bigLightOrbSpeed, stats, direction, bigDamagePercent, 1, 0);

    }

    public void HorizontalAttackTrigger()
    {
        GameObject newLightOrb = Instantiate(lightOrb, horizontalFirePoint.position, Quaternion.identity);

        newLightOrb.GetComponent<LightOrb_Controller>().SetupLightOrb(minLightOrbSpeed, stats, Vector2.right * facingDir, minDamagePercent, .3f, 0);
    }

    public bool CanUltimate() => Physics2D.OverlapCircle(transform.position, checkPlayerRadius, whatIsPlayer);

    public void ArcOrbAttackTrigger()
    {
        // Tạo hướng bay ngẫu nhiên lên trời:
        float xSpeed = Random.Range(-2f, 2f); // bay hơi lệch trái/phải
        float ySpeed = Random.Range(4f, 6f);  // bay lên cao

        Vector2 dir = new Vector2(xSpeed, ySpeed);

        GameObject newLightOrb = Instantiate(lightOrb, firePoint.position, Quaternion.identity);

        float randomScale = Random.Range(.2f, .7f);

        lightOrbSize = randomScale;

        newLightOrb.GetComponent<LightOrb_Controller>().SetupLightOrb(arcOrbSpeed, stats, dir, arcOrbDMG, lightOrbSize, 1f);
    }

    public void ExplosionTrigger()
    {
        GameObject newExplosion = Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);

        newExplosion.GetComponent<ShadowExplosion_Controller>().SetupExplosion(this);
    }


    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);


        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            FindPosition();
        }
    
    }
    public bool PlayerInMeleeRange()
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);
        return dist <= meleeRange;
    }
    public void ShakeCamera()
    {
        impulseSource.GenerateImpulse(); // ✅ đúng

    }

    public override void Die()
    {
        base.Die();

        playableDirector.Play();
        stateMachine.ChangeState(deadState);

    }
    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
        Gizmos.DrawWireSphere(boomPoint.position, boomRadius);
        Gizmos.DrawWireSphere(transform.position, checkPlayerRadius);

        // Vẽ vùng đánh gần
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

    public void SelfDestroy() => Destroy(gameObject);
}
