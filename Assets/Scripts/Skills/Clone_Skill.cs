using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggressive Clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;

    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multiCloneUnlockButton;
    [SerializeField] private float multiCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float changeToDuplicate;

    [Header("Crystal Instead Of Clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadCloneUnlockButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multiCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    protected override void Update()
    {
        base.Update();
    }

    #region Unlock Region

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockMultiClone();
        UnlockCrystalInstead();
    }
    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (multiCloneUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInstead()
    {
        if (crystalInsteadCloneUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }
    #endregion

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        Clone_Skill_Controller cloneController = newClone.GetComponent<Clone_Skill_Controller>();

        // Klonu kur
        cloneController.SetupClone(_clonePosition, cloneDuration, canAttack,
            _offset, FindClosestEnemy(newClone.transform), canDuplicateClone,
            changeToDuplicate, player, attackMultiplier);

        // D��mana do�ru y�nlendir (HEMEN �al��s�n diye burada �a��r�yoruz)
        cloneController.FaceClosestTarget();
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        // D��man�n tam kar��s�nda (0.8f mesafede) olu�tur
        Vector3 offset = new Vector3(0.8f * player.facingDir, 0.2f);
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, offset));
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
