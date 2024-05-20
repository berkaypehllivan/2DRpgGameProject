using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        HandleJumpInput();
        HandleMovementInput();
        CheckIfFalling();
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.4f);
        }
    }

    private void HandleMovementInput()
    {
        if (xInput != 0)
        {
            rb.velocity = new Vector2(xInput * player.moveSpeed, rb.velocity.y);
            if ((xInput > 0 && player.facingDir != 1) || (xInput < 0 && player.facingDir == 1))
            {
                player.Flip();
            }
        }
    }

    private void CheckIfFalling()
    {
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
}
