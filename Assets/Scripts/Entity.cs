using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Collision Info

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 0.5f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 0.5f;
    [SerializeField] protected LayerMask whatIsGround;
    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius = 1.2f;

    #endregion

    public int knockbackDir { get; private set; }
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    [Header("Knockback Info")]
    [SerializeField] protected float knockbackDuration = 0.1f;
    [SerializeField] protected Vector2 knockbackOffset = new Vector2(2, 5);
    [SerializeField] protected Vector2 knockbackPower = new Vector2(5, 7);

    protected bool isKnocked;
    protected bool isCooldown;

    public System.Action onFlipped;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Vector2 vecGravity { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Character_Stats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<Character_Stats>();
        cd = GetComponent<CapsuleCollider2D>();

        vecGravity = new Vector2(0, -Physics2D.gravity.y);
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed() => anim.speed = 1;

    public virtual void DamageImpact()
    {
        if (!isCooldown)
        {
            StartCoroutine("DamageCooldown");
            StartCoroutine("HitKnockback");
        }
    }

    private IEnumerator DamageCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(.3f);
        isCooldown = false;
    }

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockbackDir = -1;  // Saðdan vurulursa saða gitmeli
        else
            knockbackDir = 1; // Soldan vurulursa sola gitmeli
    }


    public virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        float xOffset = Random.Range(knockbackOffset.x, knockbackOffset.y);

        Vector2 knockbackForce = new Vector2(
            (knockbackPower.x + xOffset) * knockbackDir,
            knockbackPower.y
        );

        rb.velocity = knockbackForce;

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
        rb.velocity = new Vector2(0, rb.velocity.y); // Knockback sonrasý hareketi sýfýrla
    }


    public void SetupKnockbackPower(Vector2 _knockbackPower) => knockbackPower = _knockbackPower;

    protected virtual void SetupZeroKnockbackPower()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {

        // GroundCheck için raycast çizgisi
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        // WallCheck için raycast çizgisi
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));

        // AttackCheck için bir daire
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
    }

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
            facingRight = false;
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    #endregion

    #region Velocity

    public virtual void setZeroVelocity()
    {
        if (isKnocked) return; // Knockback sýrasýnda hareketi engelle

        rb.velocity = Vector2.zero;
    }

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked) return; // Knockback sýrasýnda hareketi engelle

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }


    #endregion

    public virtual void Die()
    {

    }
}

