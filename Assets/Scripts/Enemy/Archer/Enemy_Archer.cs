using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Archer Specific Info")]
    [SerializeField] private GameObject arrow;

    public Vector2 jumpVelocity;
    public float jumpCooldown;
    public float safeDistance; // how close player should be to trigger jump on battle state
    [HideInInspector] public float lastTimeJumped;

    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherDeathState deathState { get; private set; }
    public ArcherStunnedState stunnedState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        battleState = new ArcherBattleState(this, stateMachine, "Move", this);
        deathState = new ArcherDeathState(this, stateMachine, "Death", this);
        stunnedState = new ArcherStunnedState(this, stateMachine, "Stunned", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();

        skeleton.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");

        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(deathState);
            return;
        }

        stateMachine.ChangeState(deathState);
    }
}
