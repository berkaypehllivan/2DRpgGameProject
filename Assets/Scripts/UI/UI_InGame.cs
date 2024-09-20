using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private Player_Stats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;

    [Header("Souls Info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;

    private void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skills = SkillManager.instance;


    }

    private void Update()
    {
        UpdateSoulsUI();

        if (!skills.dash.dashUnlocked)
            LockedSkillSlot(dashImage);

        if (!skills.parry.parryUnlocked)
            LockedSkillSlot(parryImage);

        if (!skills.crystal.crystalUnlocked)
            LockedSkillSlot(crystalImage);
        else
            OpenSkillSlot(crystalImage);

        if (!skills.sword.swordUnlocked)
            LockedSkillSlot(swordImage);
        else
            OpenSkillSlot(swordImage);

        if (!skills.blackHole.blackHoleUnlocked)
            LockedSkillSlot(blackHoleImage);

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.blackHole.blackHoleUnlocked)
            SetCooldownOf(blackHoleImage);

        if (Input.GetKeyDown(KeyCode.LeftAlt) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(blackHoleImage, skills.blackHole.cooldown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrency();

        currentSouls.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;

    }

    private void LockedSkillSlot(Image _image) => _image.fillAmount = 1;

    private void OpenSkillSlot(Image _image) => _image.fillAmount = 0;

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
