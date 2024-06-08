using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private Enemy_Stats enemyStats;
    private RectTransform myTransform;
    private Slider slider;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        canvasGroup = GetComponent<CanvasGroup>();
        enemyStats = GetComponentInParent<Enemy_Stats>();

        entity.onFlipped += FlipUI;
        enemyStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = enemyStats.GetMaxHealthValue();
        slider.value = enemyStats.currentHealth;

        if (enemyStats.currentHealth == enemyStats.GetMaxHealthValue())
            canvasGroup.alpha = 0;

        if (enemyStats.currentHealth < 0)
            gameObject.SetActive(false);
    }



    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        enemyStats.onHealthChanged -= UpdateHealthUI;
    }
}
