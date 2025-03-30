using System.Collections;
using UnityEngine;

public class ArcherDeathState : EnemyState
{
    private Enemy_Archer enemy;
    public ArcherDeathState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 4f;

        AudioManager.instance.PlaySFX(5, null);

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
