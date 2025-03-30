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
        AudioManager.instance.PlaySFX(13, null);

        player.fx.PlayJumpDustFx();
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
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }
    }

    private void HandleMovementInput()
    {
        if (xInput != 0)
        {
            float airControlFactor = 0.1f;
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, xInput * player.moveSpeed, airControlFactor), rb.velocity.y);

            if ((xInput > 0 && player.facingDir != 1) || (xInput < 0 && player.facingDir == 1))
                player.Flip();
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
