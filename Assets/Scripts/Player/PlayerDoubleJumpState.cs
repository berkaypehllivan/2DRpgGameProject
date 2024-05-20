using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleJumpState : PlayerState
{
    public PlayerDoubleJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();


        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce * 1f);
        player.DoubleJump = false;
        player.coyoteTimeCounter = 0f;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!player.DoubleJump)
            stateMachine.ChangeState(player.airState);
    }
}
