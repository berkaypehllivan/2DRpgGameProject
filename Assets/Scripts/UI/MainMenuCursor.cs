using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuCursor : MonoBehaviour
{
    [Header("Glow Settings")]
    [SerializeField] private Image cursorGlow;
    [SerializeField] private float glowIntensity = 1.5f;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minGlowSize = 0.8f;
    [SerializeField] private float maxGlowSize = 1.2f;
    [SerializeField] private float hoverGlowSize = 1.5f;
    [SerializeField] private float transitionSpeed = 3f;

    private RectTransform glowRect;
    private Vector2 originalSize;
    private float targetSize;
    private float currentSize;
    private bool isHovering;
    private bool isInitialized = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        // �lk frame'de do�ru boyutu ayarla
        glowRect = cursorGlow.GetComponent<RectTransform>();
        originalSize = glowRect.sizeDelta;

        // Ba�lang�� boyutunu minGlowSize yap
        currentSize = minGlowSize;
        glowRect.sizeDelta = originalSize * currentSize;

        isInitialized = true;
    }

    private void OnEnable()
    {
        // Obje aktif oldu�unda boyutu s�f�rla
        if (isInitialized)
        {
            currentSize = minGlowSize;
            glowRect.sizeDelta = originalSize * currentSize;
        }
    }

    private void Update()
    {
        // Fare pozisyonunu takip et
        cursorGlow.transform.position = Input.mousePosition;

        // Hover durumuna g�re hedef boyutu belirle
        targetSize = isHovering ? hoverGlowSize :
            Mathf.Lerp(minGlowSize, maxGlowSize, Mathf.PingPong(Time.time * pulseSpeed, 1));

        // Yumu�ak ge�i� uygula (Time.unscaledDeltaTime kullanarak men� duraklat�ld���nda da �al��s�n)
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.unscaledDeltaTime * transitionSpeed);
        glowRect.sizeDelta = originalSize * currentSize;

        // Opakl�k efekti
        float alpha = glowIntensity * (0.7f + (currentSize - minGlowSize) / (hoverGlowSize - minGlowSize) * 0.3f);
        cursorGlow.color = new Color(1, 1, 1, alpha);
    }

    public void OnButtonHover() => isHovering = true;

    public void OnButtonExit() => isHovering = false;
}
