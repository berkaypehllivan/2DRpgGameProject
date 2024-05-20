using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private Character_Stats myStats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<Character_Stats>();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
