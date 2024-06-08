using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null)
            statNameText.text = statName;
        else
            Debug.LogWarning("statNameText is not assigned.");
    }

    void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
        if (ui == null)
        {
            Debug.LogError("UI component not found in parent.");
        }
    }


    public void UpdateStatValueUI()
    {
        Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();
        if (playerStats == null)
        {
            Debug.LogError("Player_Stats component not found on player.");
            return;
        }

        switch (statType)
        {
            case StatType.health:
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
                break;
            case StatType.damage:
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
                break;
            case StatType.critPower:
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
                break;
            case StatType.critChange:
                statValueText.text = (playerStats.critChange.GetValue() + playerStats.agility.GetValue()).ToString();
                break;
            case StatType.evasion:
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
                break;
            case StatType.magicRes:
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3)).ToString();
                break;
            default:
                statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
                break;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui != null && ui.statToolTip != null)
        {
            ui.statToolTip.ShowStatToolTip(statDescription);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui != null && ui.statToolTip != null)
        {
            ui.statToolTip.HideStatToolTip();
        }
    }

}
