using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;
    private Transform closestEnemy;
    private bool canDuplicateClone;

    private float chanceToDuplicate;
    private int facingDir = 1;

    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

        if (sr.color.a <= 0)
            Destroy(gameObject);
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _changeToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 3));

        player = _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        canDuplicateClone = _canDuplicate;
        closestEnemy = _closestEnemy;
        chanceToDuplicate = _changeToDuplicate;
        attackMultiplier = _attackMultiplier;

        if (player.dashDir != 0)
        {
            facingDir = (int)Mathf.Sign(_player.dashDir);
            sr.flipX = facingDir < 0;
        }
        else
            FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTriggers()
    {
        AudioManager.instance.PlaySFX(1, null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.stats.DoDamage(hit.GetComponent<Character_Stats>());

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                Player_Stats playerStats = player.GetComponent<Player_Stats>();
                Enemy_Stats enemyStats = hit.GetComponent<Enemy_Stats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                }
            }
        }
    }

    public void FaceClosestTarget()
    {
        // 3 birim mesafe kontrolü
        if (closestEnemy != null && Vector3.Distance(transform.position, closestEnemy.position) <= 10f)
        {
            // Sadece sprite'ý çevir, rotation kullanma
            sr.flipX = closestEnemy.position.x < transform.position.x;
        }
    }
}
