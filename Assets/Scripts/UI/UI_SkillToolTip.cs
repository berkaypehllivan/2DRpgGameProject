using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontSize;

    public void ShowToolTip(string _skillDescription, string _skillName, int _price)
    {
        skillDescription.text = _skillDescription;
        skillName.text = _skillName;
        skillCost.text = "Fiyat: " + _price;
        gameObject.SetActive(true);

        AdjustPosition();
        AdjustFontSize(skillName);
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
