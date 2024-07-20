using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stats : Character_Stats
{

    private Enemy enemy;
    private ItemsDrop myDropSystem;
    public Stat soulsDropAmount;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = .4f;


    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(500);

        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemsDrop>();

    }

    private void ApplyLevelModifiers()
    {
        Modify(damage);
        Modify(critChange);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        if (currentHealth > 10)
            AudioManager.instance.PlaySFX(3, null);

        healthBarUI.canvasGroup.alpha = 1;
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.GetValue();

        myDropSystem.GenerateDrop();
    }
}
