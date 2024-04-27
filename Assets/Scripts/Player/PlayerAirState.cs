using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackHole);

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSword);

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (player.DoubleJump && Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.doubleJump);

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);

        if (rb.velocity.y < 0)
            rb.velocity -= player.vecGravity * player.fallMultiplier * Time.deltaTime / 2;
    }

    private bool HasNoSword()
    {
        if (!player.sword)
            return true;

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
