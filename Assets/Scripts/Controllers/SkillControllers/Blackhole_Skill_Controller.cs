using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;

    private bool canShrink;
    private bool cloneAttackReleased;
    private bool canGrow = true;
    private bool canCreateHotKeys = true;
    private bool playerCanDissapear = true;

    private float cloneAttackTimer;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;

    public List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkspeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkspeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDissapear = false;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {

        if (targets.Count <= 0)
            return;

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if (playerCanDissapear)
        {
            playerCanDissapear = false;
            PlayerManager.instance.player.fx.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (targets != null && targets.Count > 0 && cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);
            Transform target = targets[randomIndex];

            // Düþmanýn tam karþýsýnda ve biraz uzaðýnda oluþtur
            float xDirection = Random.Range(0, 2) == 0 ? -1 : 1; // -1 veya 1
            float xDistance = Random.Range(0.8f, 1.2f);
            Vector3 offset = new Vector3(xDistance * xDirection, 0.2f, 0);

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
                amountOfAttacks--;
            }
            else
            {
                SkillManager.instance.clone.CreateClone(target, offset);
                amountOfAttacks--;
            }

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", 1.5f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();
        canShrink = true;
        playerCanExitState = true;
        cloneAttackReleased = false;
    }

    private void DestroyHotKeys()
    {
        if (createdHotKey != null && createdHotKey.Count > 0)
        {
            for (int i = 0; i < createdHotKey.Count; i++)
            {
                if (createdHotKey[i] != null) // Null kontrolü
                    Destroy(createdHotKey[i]);
            }

            createdHotKey.Clear(); // Listeyi temizleyin
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sadece belirli bir layer'daki düþmanlarý algýla
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            return;

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemy.isFrozen) // Ek kontrol
        {
            enemy.FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

    private void CreateHotKey(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null || enemy.hasHotKey) return;

        enemy.hasHotKey = true;

        if (keyCodeList.Count <= 0)
        {
            Debug.Log("Not enough hot keys in a key code list!");
            return;
        }

        if (!canCreateHotKeys)
            return;

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_Hotkey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);

    public void RemoveEnemyToList(Transform _enemyTransform) => targets.Remove(_enemyTransform);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxSize); // Blackhole'in büyüklüðünü görselleþtirin
    }

}
