using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Skill : Skill
{

    [SerializeField] private bool canInvincibleOnDash;

    public override void UseSkill()
    {
        base.UseSkill();

        Debug.Log("Created clone behind");
    }

    public void CanInvincibleOnDash()
    {
        if (canInvincibleOnDash)
            player.col.enabled = false;
    }
}
