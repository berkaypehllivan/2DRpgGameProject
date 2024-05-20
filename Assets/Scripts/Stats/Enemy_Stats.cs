using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stats : Character_Stats
{

    private Enemy enemy;


    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();
    }
}
