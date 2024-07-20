using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeathState : EnemyState
{
    private Enemy_Slime enemy;
    public SlimeDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 4f;

        AudioManager.instance.PlaySFX(20, null);

    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 2)
        {
            enemy.cd.enabled = false;

            if (stateTimer < 0)
                enemy.DestroyGameObject();
        }

    }
}
