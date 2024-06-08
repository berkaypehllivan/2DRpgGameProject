using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        HandleStateTransitions();
        HandleMovementInput();
        ApplyGravity();
        HandleJumpInput();
    }

    private void HandleStateTransitions()
    {
        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackHole.blackHoleUnlocked)
        {
            stateMachine.ChangeState(player.blackHole);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
        {
            stateMachine.ChangeState(player.aimSword);
            return;
        }

        if (player.IsWallDetected() && player.canWallSlide && player.skill.wallMoving.wallSlideUnlock)
        {
            stateMachine.ChangeState(player.wallSlide);
            return;
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (player.DoubleJump && Input.GetKeyDown(KeyCode.Space) && player.skill.doubleJump.doubleJumpUnlocked)
        {
            stateMachine.ChangeState(player.doubleJump);
            return;
        }

        if (player.coyoteTimeCounter > 0f && Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }
    }

    private void HandleMovementInput()
    {
        if (xInput != 0)
        {
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.velocity.y);
        }
    }

    private void ApplyGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity -= player.vecGravity * player.fallMultiplier * Time.deltaTime / 2;
        }
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.4f);
        }
    }

    private bool HasNoSword()
    {
        if (!player.sword)
            return true;

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
