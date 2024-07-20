using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(26, null);

        GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen();

        player.gameObject.layer = LayerMask.NameToLayer("Enemy");
        player.gameObject.tag = "Enemy";
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.setZeroVelocity();
    }
}
