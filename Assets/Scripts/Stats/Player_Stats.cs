using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Character_Stats
{
    public event System.Action OnHealthChanged;

    private Player player;
    [SerializeField] private float cooldown;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();

        cooldown -= Time.deltaTime;
    }

    public override void TakeDamage(int _damage)
    {
        if (!isDead)
        {
            if (cooldown > 0)
            {
                return;
            }
        }

        base.TakeDamage(_damage);

        cooldown = 1;

        AudioManager.instance.PlaySFX(17, null);

        player.fx.ScreenShake(player.fx.shakeDamageImpact);

    }

    public override void DoDamage(Character_Stats _targetStats)
    {
        base.DoDamage(_targetStats);

        player.fx.ScreenShake(player.fx.shakeDamageImpact);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItem_Drop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        OnHealthChanged?.Invoke();

        if (_damage > GetMaxHealthValue() * .3f)
        {
            player.SetupKnockbackPower(new Vector2(10, 6));
            player.fx.ScreenShake(player.fx.shakeHighImpact);
        }

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    public void CloneDoDamage(Character_Stats _targetStats, float _multiplier)
    {

        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);
    }
}
