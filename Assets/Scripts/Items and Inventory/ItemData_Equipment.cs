using System.Collections.Generic;
using System.Net;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique Effects")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive Stats")]
    public int damage;
    public int critChange;
    public int critPower;

    [Header("Deffensive Stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;

    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void AddModifiers()
    {
        Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChange.AddModifier(critChange);
        playerStats.critPower.AddModifier(armor);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChange.RemoveModifier(critChange);
        playerStats.critPower.RemoveModifier(armor);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public string GetEquipmentType()
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon:
                return "K�l��";
            case EquipmentType.Armor:
                return "Z�rh";
            case EquipmentType.Amulet:
                return "T�ls�m";
            case EquipmentType.Flask:
                return "�ksir";
            default:
                return "Bilinmeyen";
        }
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "G��");
        AddItemDescription(agility, "�eviklik");
        AddItemDescription(intelligence, "Zeka");
        AddItemDescription(vitality, "Canl�l�k");

        AddItemDescription(damage, "Hasar");
        AddItemDescription(critChange, "Krt. �ans�");
        AddItemDescription(critPower, "Krt. G�c�");

        AddItemDescription(health, "Sa�l�k");
        AddItemDescription(armor, "Z�rh");
        AddItemDescription(evasion, "Ka��nma");
        AddItemDescription(magicResistance, "B�y� Direnci");

        AddItemDescription(fireDamage, "Ate� Hasar�");
        AddItemDescription(iceDamage, "Buz Hasar�");
        AddItemDescription(lightingDamage, "Y�ld�r�m Hasar�");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i] != null)
            {
                if (itemEffects[i].effectDescription.Length > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine("<color=#FFBF00>Epik:</color> " + itemEffects[i].effectDescription);
                    descriptionLength++;
                }
            }

        }

        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }



        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (_value > 0)
                sb.Append(" + " + _value + " " + _name);

            descriptionLength++;
        }
    }
}
