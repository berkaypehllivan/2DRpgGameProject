using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private Character_Stats myStats;
    private RectTransform myTransform;
    private Slider slider;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<Character_Stats>();
        canvasGroup = GetComponent<CanvasGroup>();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;

        if (myStats.currentHealth == myStats.GetMaxHealthValue())
        {
            canvasGroup.alpha = 0;
        }
    }



    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
