using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallMoving_Skill : Skill
{
    [Header("Wall Slide")]
    [SerializeField] private UI_SkillTreeSlot wallSlideUnlockButton;
    public bool wallSlideUnlock;

    [Header("Wall Jump")]
    [SerializeField] private UI_SkillTreeSlot wallJumpUnlockButton;
    public bool wallJumpUnlock;

    protected override void Start()
    {
        base.Start();

        wallSlideUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockWallSlide);
        wallJumpUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockWallJump);
    }

    protected override void CheckUnlock()
    {
        UnlockWallSlide();
        UnlockWallJump();
    }
    private void UnlockWallSlide()
    {
        if (wallSlideUnlockButton.unlocked)
        {
            wallSlideUnlock = true;
            player.canWallSlide = true;

        }
    }

    private void UnlockWallJump()
    {
        if (wallJumpUnlockButton.unlocked)
        {
            wallJumpUnlock = true;
            player.canWallJump = true;
        }
    }
}
