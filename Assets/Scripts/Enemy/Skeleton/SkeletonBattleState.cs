using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private int moveDir;

    private Player myPlayer;
    private EnemySkeleton enemy;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        myPlayer = PlayerManager.instance.player;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (myPlayer.isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
            return;
        }
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (canAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }
        UpdateMoveDirection();

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private void UpdateMoveDirection()
    {
        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;
    }

    private bool canAttack()
    {
        if (Time.time >= enemy.lasTimeAttacked + enemy.attackCooldown)
        {
            enemy.lasTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
