using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicUltimate_Skill : Skill
{
    [Header("Ultimate")]
    [SerializeField] UI_SkillTreeSlot physicUltimateUnlockButton;
    public bool physicUltimateUnlocked { get; private set; }
    public int damageIncrease;
    public float damageBuffPercent;


    [Header("Fire Sword")]
    [SerializeField] UI_SkillTreeSlot fireSwordUnlockButton;
    public bool fireSwordUnlocked { get; private set; }
    public bool isFireSword;
    public float fireSwordTime;
    private float fireSwordTimer;
    public float damageBuffPercentFireSword;

    [Header("Increase 100% crit rate")]
    [SerializeField] UI_SkillTreeSlot increaseCritRateUnlockButton;
    public bool increaseCritRateUnlocked {  get; private set; }


    protected override void Start()
    {
        base.Start();
        fireSwordTimer = fireSwordTime;

        damageIncrease = Mathf.RoundToInt(PlayerManager.instance.player.stats.damage.GetValue() * damageBuffPercent);
        physicUltimateUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPhysicUltimate);
        fireSwordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockFireSword);
        increaseCritRateUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockIncreaseCritRate);
    }

    protected override void Update()
    {
        base.Update();

        if (isFireSword)
        {

            fireSwordTimer -= Time.deltaTime;
        }

        if(fireSwordTimer <= 0)
        {
            fireSwordTimer = fireSwordTime;
            PlayerManager.instance.player.ExitFireSwordMode();
        }
    }

    protected override void CheckUnlock()
    {
        UnlockPhysicUltimate();
        UnlockFireSword();
        UnlockIncreaseCritRate();
    }

    private void UnlockPhysicUltimate()
    {
        if(physicUltimateUnlockButton.unlocked)
            physicUltimateUnlocked = true;
    }

    private void UnlockFireSword()
    {
        if(fireSwordUnlockButton.unlocked)
            fireSwordUnlocked = true;
    }

    private void UnlockIncreaseCritRate()
    {
        if(increaseCritRateUnlockButton.unlocked)
            increaseCritRateUnlocked = true;
    }

}
