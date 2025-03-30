using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(11, null);

        player.skill.dash.CloneOnDash();
        player.skill.dash.InvincibleDash();
        stateTimer = player.dashDuration;

        if (player.IsGroundDetected())
            player.fx.PlayMovementDustFx();

    }

    public override void Exit()
    {
        base.Exit();

        player.skill.dash.CloneOnArrival();
        player.col.isTrigger = false;

        player.SetVelocity(rb.velocity.x, rb.velocity.y);

        if (!player.IsGroundDetected())
            player.SetVelocity(0f, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected() && player.canWallJump && !player.col.isTrigger)
            stateMachine.ChangeState(player.wallSlide);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.moveState);

        if (player.skill.dash.invincibleOnDashUnlocked)
            player.fx.CreateAfterImage();
    }
}
