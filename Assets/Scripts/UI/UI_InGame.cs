using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;

    [SerializeField] private Player_Stats playerStats;

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

    [SerializeField] private Slider instantHealthBar; // Kýrmýzý - anlýk can
    [SerializeField] private Slider delayedHealthBar; // Sarý - yavaþ azalan can
    [SerializeField] private float damageFollowSpeed = 3f; // Sarý barýn takip hýzý

    [Header("Flash Settings")]
    [SerializeField] private float flaskCooldown;
    private float flaskCooldownTimer;
    private bool isFlaskCooldown;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skills = SkillManager.instance;

        InitializeFlaskCooldown();
    }

    private void Update()
    {
        UpdateSoulsUI();
        UpdateHealthUI();

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

        if (Input.GetKeyDown(KeyCode.LeftAlt) && CanUseFlask())
        {
            SetCooldownOf(flaskImage);
            StartFlaskCooldown();
        }

        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(blackHoleImage, skills.blackHole.cooldown);

        UpdateFlaskCooldown();
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
        // Max deðerleri güncelle
        float maxHealth = playerStats.GetMaxHealthValue();
        instantHealthBar.maxValue = maxHealth;
        delayedHealthBar.maxValue = maxHealth;

        // Kýrmýzý barý anýnda güncelle
        instantHealthBar.value = playerStats.currentHealth;

        // Sarý bar sadece hasar alýndýðýnda yavaþça takip etsin
        if (delayedHealthBar.value > instantHealthBar.value)
        {
            delayedHealthBar.value = Mathf.MoveTowards(delayedHealthBar.value,
                                                     instantHealthBar.value,
                                                     damageFollowSpeed * Time.deltaTime);
        }
        // Can doluyorsa anýnda güncellensin
        else
        {
            delayedHealthBar.value = instantHealthBar.value;
        }
    }

    #region Flask

    private bool CanUseFlask()
    {
        ItemData_Equipment flask = Inventory.instance.GetEquipment(EquipmentType.Flask);

        if (flask == null)
            return false;

        if (isFlaskCooldown)
            return false;

        if (playerStats.currentHealth >= playerStats.GetMaxHealthValue())
        {
            Debug.Log("Flask Kullanýlabilmesi için saðlýðýn düþük olmasý gerekiyor!");
            return false;
        }

        return true;
    }

    private void InitializeFlaskCooldown()
    {
        ItemData_Equipment currentFlask = Inventory.instance.GetEquipment(EquipmentType.Flask);
        if (currentFlask != null)
        {
            flaskImage.fillAmount = 1;
            isFlaskCooldown = true;
            flaskCooldown = currentFlask.itemCooldown;
            flaskCooldownTimer = flaskCooldown;
        }
        else
        {
            flaskImage.fillAmount = 0;
        }
    }

    private void UpdateFlaskCooldown()
    {
        if (!isFlaskCooldown)
        {
            flaskImage.fillAmount = 0;
            return;
        }

        flaskCooldownTimer -= Time.deltaTime;
        flaskImage.fillAmount = flaskCooldownTimer / flaskCooldown;

        if (flaskCooldownTimer <= 0)
        {
            isFlaskCooldown = false;
            flaskImage.fillAmount = 0;
        }
    }

    public void StartFlaskCooldown()
    {
        ItemData_Equipment currentFlask = Inventory.instance.GetEquipment(EquipmentType.Flask);
        if (currentFlask != null)
        {
            flaskCooldown = currentFlask.itemCooldown;
            flaskCooldownTimer = flaskCooldown;
            isFlaskCooldown = true;
            flaskImage.fillAmount = 1; // Görseli tam dolu yap
        }
    }

    #endregion

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
