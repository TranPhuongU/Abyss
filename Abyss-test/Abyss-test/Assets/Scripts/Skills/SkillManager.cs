using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;


    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public Sword_Skill sword { get; private set; }
    public SilentWorld_Skill silentWorld { get; private set; }
    public Crystal_Skill crystal { get; private set; }
    public Parry_Skill parry { get; private set; }
    public Dodge_Skill dodge { get; private set; }
    public SlashWave_Skill slashWave { get; private set; }
    public PhysicUltimate_Skill physicUltimate { get; private set; }
    public Rewind_Skill rewind { get; private set; }
    public Buff_Skill buff { get; private set; }
    public Morph_Skill morph {  get; private set; }

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
        sword = GetComponent<Sword_Skill>();
        silentWorld = GetComponent<SilentWorld_Skill>();
        crystal = GetComponent<Crystal_Skill>();
        parry = GetComponent<Parry_Skill>();
        dodge = GetComponent<Dodge_Skill>();
        slashWave = GetComponent<SlashWave_Skill>();
        physicUltimate = GetComponent<PhysicUltimate_Skill>();
        rewind = GetComponent<Rewind_Skill>();
        buff = GetComponent<Buff_Skill>();
        morph = GetComponent<Morph_Skill>();
    }
}
