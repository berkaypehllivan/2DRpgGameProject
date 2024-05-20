using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState : EnemyState
{
    private EnemySkeleton enemy;
    public SkeletonDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 2f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            enemy.cd.enabled = false;

    }
}
