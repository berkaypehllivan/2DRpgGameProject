using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleJump_Skill : Skill
{
    [Header("Double Jump")]
    [SerializeField] private UI_SkillTreeSlot doubleJumpUnlockButton;
    public bool doubleJumpUnlocked;

    protected override void Start()
    {
        base.Start();

        doubleJumpUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDoubleJump);
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void UnlockDoubleJump()
    {
        if (doubleJumpUnlockButton.unlocked)
        {
            doubleJumpUnlocked = true;
            player.canDoubleJump = true;
        }
    }
}
