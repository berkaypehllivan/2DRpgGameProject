using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Character_Stats
{

    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    public override void DoDamage(Character_Stats _targetStats)
    {
        base.DoDamage(_targetStats);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }
}
