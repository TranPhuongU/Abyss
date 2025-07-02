using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


public class Player : Entity
{
    private CinemachineImpulseSource impulseSource;

    [SerializeField] GameObject randomScorePrefab;

    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private Transform smokeSpawnPoint;

    [Header("Ledge info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    public Vector2 climbBeginPosition;
    public Vector2 climbOverPosition;
    public bool canGrabLedge = true;
    public bool canClimb;
    [HideInInspector] public bool ledgeDetected;


    [Header("Ultimate info")]
    [SerializeField] AnimatorOverrideController normalAnim;
    [SerializeField] AnimatorOverrideController fireAnim;
    public GameObject darkScreenUltimate;

    [Header("Form info")]
    [SerializeField] RuntimeAnimatorController form1Anim;
    [SerializeField] RuntimeAnimatorController form2Anim;

    [Header("Transform info")]
    public bool isForm2;

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;


    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    public float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashResetDelay = 1f;
    public float dashResetTimer = 0f;
    public int consecutiveDashCount = 0;
    public bool canDash;
    public bool dashing;
    public float CurrentDashEnergy;
    public float maxDashEnergy;
    public float dashEnergyRegenRate;
    public float dashEnergyCost;
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;

    public System.Action onEnergyChanged;
    public System.Action onIceEnergyChanged;
    public float dashDir { get; private set; }

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    public PlayerFx fx { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerRewindState rewindState { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerAirDashState airDashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerPhysicUltimateState physicUltimateState { get; private set; }
    public PlayerAimSwordState aimSowrd { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackholeState blackHole { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerTransformState transformState { get; private set; }
    public PlayerClimbState climbState { get; private set; }
    #endregion



    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        airDashState = new PlayerAirDashState(this, stateMachine, "AirDash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "Idle");
        aimSowrd = new PlayerAimSwordState(this, stateMachine, "Aim");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
        physicUltimateState = new PlayerPhysicUltimateState(this, stateMachine, "PhysicUltimate");
        transformState = new PlayerTransformState(this, stateMachine, "Transform");
        rewindState = new PlayerRewindState(this, stateMachine, "Idle");
        climbState = new PlayerClimbState(this, stateMachine, "Climb");

    }

    protected override void Start()
    {
        base.Start();

        canDash = true;

        CurrentDashEnergy = maxDashEnergy;

        isForm2 = false;

        fx = GetComponent<PlayerFx>();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }


    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        UpdateDashEnergy();

        if (Time.timeScale == 0)
            return;

        CheckForDashInput();

        if (canClimb)
            stateMachine.ChangeState(climbState);


        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();

        if (Input.GetKeyDown(KeyCode.R) && skill.physicUltimate.CanUseSkill() && !isForm2 && skill.physicUltimate.physicUltimateUnlocked && canDash)
        {
            stateMachine.ChangeState(physicUltimateState);
        }

        if (Input.GetKeyDown(KeyCode.E) && !isForm2 && skill.buff.CanUseSkill()) // SKill đã được dùng trong CanUseSkill 
        {
            if (skill.buff.buffUnlocked)
            {
                GameObject newRandomFx = Instantiate(randomScorePrefab, new Vector3(transform.position.x, transform.position.y + 2), Quaternion.identity);
                newRandomFx.GetComponent<RandomBuffFx>().SetupRandomBuffFx(skill.buff.randomDuration);
            }
        }

        CheckForLedge();

    }

    public void LedgeClimbOver()
    {
        canClimb = false;

        transform.position = climbOverPosition;
        Invoke("AllowLedge", .1f);
    }

    private void AllowLedge() => canGrabLedge = true;

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            if (facingDir == -1)
            {
                climbBeginPosition = ledgePosition + new Vector2(facingDir * offset1.x, -facingDir * offset1.y);
            }
            else if (facingDir == 1)
            {
                climbBeginPosition = ledgePosition + facingDir * offset1;
            }

            if (facingDir == -1)
            {
                climbOverPosition = ledgePosition + new Vector2(facingDir * offset2.x, -facingDir * offset2.y);
            }
            else if (facingDir == 1)
            {

                climbOverPosition = ledgePosition + (facingDir * offset2);
            }

            canClimb = true;

        }

    }

    private void UpdateDashEnergy()
    {
        if (CurrentDashEnergy < 100)
        {
            CurrentDashEnergy += dashEnergyRegenRate * Time.deltaTime;

            if (onEnergyChanged != null)
                onEnergyChanged();
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);

    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        // Reset số lần dash nếu đã quá thời gian cho phép
        if (consecutiveDashCount > 0)
        {
            dashResetTimer += Time.deltaTime;
            if (dashResetTimer >= dashResetDelay)
            {
                consecutiveDashCount = 0;
                dashResetTimer = 0f;
            }
        }

        if (IsWallDetected())
            return;

        if (!skill.dash.dashUnlocked)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1) && CurrentDashEnergy > 0 && canDash)
        {
            // Nếu đã dash 2 lần liên tục thì không cho dash nữa
            if (consecutiveDashCount >= 2)
                return;

            // Trừ năng lượng và tăng số lần dash liên tục
            CurrentDashEnergy -= dashEnergyCost;
            

            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;

            onEnergyChanged?.Invoke();

            if (IsGroundDetected())
            {
                stateMachine.ChangeState(dashState);
            }
            else
            {
                stateMachine.ChangeState(airDashState);
            }
        }
    }

public override void Die()
    {
        base.Die();

        //stats.currentHealth = 0;
        //stats.onHealthChanged();

        if (isForm2 && skill.rewind.cooldownTimer <= 0)
        {
            if (skill.rewind.rewindUnlocked)
            {
                stateMachine.ChangeState(rewindState);
                return;
            }
        }

        stateMachine.ChangeState(deadState);
    }
    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
    public void EnterFireSwordMode()
    {
        if (skill.physicUltimate.fireSwordUnlocked)
        {
            anim.runtimeAnimatorController = fireAnim;
            skill.physicUltimate.isFireSword = true;
            stats.IncreaseStatBy(skill.physicUltimate.damageBuffPercentFireSword, skill.physicUltimate.fireSwordTime, stats.GetStat(StatType.damage));
        }
    }

    public void ExitFireSwordMode()
    {
        anim.runtimeAnimatorController = normalAnim;
        skill.physicUltimate.isFireSword = false;
        stats.critChance.RemoveModifier(100);
        stats.critPower.RemoveModifier(100);
    }
    public void Transform()
    {
        if (isForm2)
        {
            anim.runtimeAnimatorController = form2Anim;
        }
        else if (skill.physicUltimate.isFireSword)
        {
            anim.runtimeAnimatorController = fireAnim;
        }
        else if(!isForm2 && !skill.physicUltimate.isFireSword)
        {
            anim.runtimeAnimatorController = form1Anim;
        }
    }

    public bool HasNoSword()
    {
        if (!sword)
        {
            return true;
        }

        //player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }

    public void HidePlayerForCinematic()
    {
        // 2. Tắt renderer
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Player>().enabled = false;

    }

    public void ShowPlayerForCinematic()
    {
        // 2. Tắt renderer
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<Player>().enabled = true;

    }

    public void ResetPlayer()
    {
        stats.isDead = false;
        stateMachine.ChangeState(idleState); // hoặc gọi hàm nào đưa về idle
        stats.currentHealth = Mathf.RoundToInt(stats.maxHealth.GetValue() * 0.5f) ;

        if (stats.onHealthChanged != null)
            stats.onHealthChanged();
    }

    public void CreateLandingSmoke()
    {
        if (smokePrefab != null && smokeSpawnPoint != null)
        {
            Instantiate(smokePrefab, smokeSpawnPoint.position, Quaternion.identity);
        }
    }

    public override void SetupKnockbackPower(Vector2 _knockbackpower)
    {
        base.SetupKnockbackPower(_knockbackpower);

        impulseSource.GenerateImpulse(); // ✅ đúng

    }

}
