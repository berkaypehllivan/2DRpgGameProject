using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public SwordSkill sword { get; private set; }
    public Blackhole_Skill blackHole { get; private set; }
    public Crystal_Skill crystal { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<SwordSkill>();
        blackHole = GetComponent<Blackhole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
    }
}
