using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private float acceleration = 25f; // Hýzlanma hýzý
    private float deceleration = 30f; // Yavaþlama hýzý
    private float smoothStopFactor = 0.1f; // Ani duruþlarý engellemek için

    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(14, null);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(14);
    }

    public override void Update()
    {
        base.Update();

        float targetSpeed = xInput * player.moveSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;
        float accelerationRate = Mathf.Abs(targetSpeed) > 0.1f ? acceleration : deceleration;

        // Hýzý ivmeli þekilde güncelle
        rb.velocity = new Vector2(rb.velocity.x + speedDifference * accelerationRate * Time.deltaTime, rb.velocity.y);

        if (xInput > 0 && player.facingDir != 1)
        {
            player.fx.PlayMovementDustFx();
            player.Flip();
        }
        else if (xInput < 0 && player.facingDir != -1)
        {
            player.fx.PlayMovementDustFx();
            player.Flip();
        }

        // Hareket duruyorsa yavaþlat
        if (xInput == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * (1 - smoothStopFactor), rb.velocity.y);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
                stateMachine.ChangeState(player.idleState);
        }

        // Duvara çarptýðýnda dur
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}


