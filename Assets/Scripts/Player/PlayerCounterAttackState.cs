using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.setZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {

            if (hit.GetComponent<Arrow_Controller>() != null)
            {
                SuccessfullCounterAttack();
                hit.GetComponent<Arrow_Controller>().FlipArrow();
            }
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    SuccessfullCounterAttack();

                    player.skill.parry.UseSkill();

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    private void SuccessfullCounterAttack()
    {
        stateTimer = 10;
        player.anim.SetBool("SuccessfulCounterAttack", true);

        AudioManager.instance.PlaySFX(10, null);
    }
}
