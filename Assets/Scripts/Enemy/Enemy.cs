using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Enemy_Stats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemsDrop))]
public class Enemy : Entity
{
    [HideInInspector] public EnemySkeleton skeleton;
    [HideInInspector] public Enemy_Slime slime;

    [Header("Stunned Info")]
    public float stunDuration = 1;
    public Vector2 stunDirection = new Vector2(4, 6);
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed = 2;
    public float idleTime = 1;
    public float battleTime = 7;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float agroDistance = 2;
    public float attackDistance = 2;
    public float attackCooldown = 0.4f;
    public float minAttackCooldown = 1;
    public float maxAttackCooldown = 2;
    [SerializeField] protected LayerMask whatIsPlayer;
    [HideInInspector] public float lasTimeAttacked;

    public EntityFX fx { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;

        skeleton = GetComponent<EnemySkeleton>();
        slime = GetComponent<Enemy_Slime>();
    }

    protected override void Start()
    {
        base.Start();

        fx = GetComponent<EntityFX>();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }


    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    public virtual void DestroyGameObject()
    {
        if (GetComponent<Enemy_Stats>().isDead)
            Destroy(gameObject);
    }

    #region Counter Attack
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual void AnimationSpecialAttackTrigger()
    {

    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 10, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
