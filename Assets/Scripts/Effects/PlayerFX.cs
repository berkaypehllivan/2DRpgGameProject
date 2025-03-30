using Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After Image FX")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float afterImageCooldown;
    [SerializeField] private float colorLooseRate;
    private float afterImageCooldownTimer;

    [Header("Screen Shake FX")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeDamageImpact;
    public Vector3 shakeHighImpact;
    [Space]
    [SerializeField] private ParticleSystem swordDustFx;
    [SerializeField] private ParticleSystem movementDustFx;
    [SerializeField] private ParticleSystem jumpDustFx;

    protected override void Start()
    {
        base.Start();

        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
        }
    }

    public void PlayMovementDustFx() => movementDustFx.Play();

    public void PlayJumpDustFx() => jumpDustFx.Play();

    public void PlaySwordDustFX()
    {
        if (swordDustFx != null)
            swordDustFx.Play();
    }
}
