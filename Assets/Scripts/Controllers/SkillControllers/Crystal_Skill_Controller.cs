using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExitsTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    private bool canGrow;
    private float growSpeed = 5;
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    public Animator anim => GetComponent<Animator>();
    private Transform closestEnemy;
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy, Player _player)
    {
        player = _player;
        crystalExitsTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.GetBlackholeRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExitsTimer -= Time.deltaTime;

        if (crystalExitsTimer < 0)
            ExplodeCrystal();

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

        if (canMove && closestEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if (closestEnemy != null && Vector2.Distance(transform.position, closestEnemy.position) < 1)
            {
                ExplodeCrystal();
                canMove = false;
            }
        }

    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                player.stats.DoMagicalDamage(hit.GetComponent<Character_Stats>());
        }
    }

    public void ExplodeCrystal()
    {
        if (canExplode)
        {
            anim.SetTrigger("Explode");
            canGrow = true;
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
