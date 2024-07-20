using System.Collections;
using UnityEngine;

public class Player : Entity
{

    #region Headers

    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    private int touchDamage = 15;

    [Header("Move Info")]
    public float moveSpeed = 12f;
    private float defaultMoveSpeed;
    public float swordReturnImpact;

    [Header("Jump Info")]
    public bool canDoubleJump;
    public bool canWallSlide;
    public bool canWallJump;
    public float jumpForce;
    private float defaultJumpForce;
    public float coyoteTime;
    [HideInInspector] public float coyoteTimeCounter;
    [HideInInspector] public float fallMultiplier;
    public bool DoubleJump;


    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;

    [HideInInspector] public Collider2D col;

    public float dashDir { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    public PlayerFX fx { get; private set; }

    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackholeState blackHole { get; private set; }
    public PlayerDoubleJumpState doubleJump { get; private set; }
    public PlayerDeathState deathState { get; private set; }

    #endregion

    public bool isBusy { get; private set; }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        blackHole = new PlayerBlackholeState(this, stateMachine, "Jump");
        doubleJump = new PlayerDoubleJumpState(this, stateMachine, "Jump");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        deathState = new PlayerDeathState(this, stateMachine, "Die");

    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<PlayerFX>();

        stateMachine.Initialize(idleState);

        skill = SkillManager.instance;

        col = GetComponent<CapsuleCollider2D>();

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (Time.timeScale == 0)
            return;

        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
            skill.crystal.CanUseSkill();

        if (!IsGroundDetected())
            coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            Inventory.instance.UseFlask();

    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
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

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (!skill.dash.dashUnlocked)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill() && !stats.isDead)
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && !stats.isDead)
        {
            SetupKnockbackPower(new Vector2(8, 10));
            stats.TakeDamage(touchDamage);
            fx.CreateHitFX(PlayerManager.instance.player.transform, false);
            fx.ScreenShake(fx.shakeHighImpact);
        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deathState);

    }
}
