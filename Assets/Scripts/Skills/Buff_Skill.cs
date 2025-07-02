using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff_Skill : Skill
{

    [Header("Buff 50%")]
    [SerializeField] UI_SkillTreeSlot buff50UnlockButton;
    public bool buff50Unlocked {  get; private set; }

    [Header("Buff 60%")]
    [SerializeField] UI_SkillTreeSlot buff60UnlockButton;
    public bool buff60Unlocked {  get; private set; }

    [Header("Buff 70%")]
    [SerializeField] UI_SkillTreeSlot buff70UnlockButton;
    public bool buff70Unlocked {  get; private set; }

    [Header("Buff skill")]
    [SerializeField] UI_SkillTreeSlot buffUnlockButton;
    public bool buffUnlocked {  get; private set; }

    [SerializeField] GameObject auraBuffDamagePrefab;
    [SerializeField] GameObject auraBuffCritRatePrefab;
    [SerializeField] GameObject auraBuffCritPowerPrefab;
    [SerializeField] GameObject auraBuffArmorPrefab;
    [SerializeField] GameObject auraBuffEvasionPrefab;
    
    [SerializeField] float buffDuration;
    public float randomDuration;

    [SerializeField] float buffPercent;
  
    public int currentScore;

    protected override void Start()
    {
        base.Start();

        buffUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBuff);
        buff50UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBuff50);
        buff60UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBuff60);
        buff70UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBuff70);
    }
    protected override void Update()
    {
        base.Update(); 

    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (buffUnlocked)
        {
            currentScore = Random.Range(1, 6);
            Invoke("Buff", 1.5f);
        }
    }

    private void Buff()
    {
        if (currentScore == 1)
        {
            player.stats.IncreaseStatBy(buffPercent, buffDuration, player.stats.damage);
            GameObject newAura = Instantiate(auraBuffDamagePrefab, player.transform.position, Quaternion.identity);
            newAura.GetComponent<AuraFx>().SetupAura(buffDuration);
        }
        else if (currentScore == 2)
        {
            player.stats.IncreaseStatBy(buffPercent, buffDuration, player.stats.critChance);
            GameObject newAura = Instantiate(auraBuffCritRatePrefab, player.transform.position, Quaternion.identity);
            newAura.GetComponent<AuraFx>().SetupAura(buffDuration);
        }
        else if (currentScore == 3)
        {
            player.stats.IncreaseStatBy(buffPercent, buffDuration, player.stats.critPower);
            GameObject newAura = Instantiate(auraBuffCritPowerPrefab, player.transform.position, Quaternion.identity);
            newAura.GetComponent<AuraFx>().SetupAura(buffDuration);
        }
        else if (currentScore == 4)
        {
            player.stats.IncreaseStatBy(buffPercent, buffDuration, player.stats.armor);
            GameObject newAura = Instantiate(auraBuffArmorPrefab, player.transform.position, Quaternion.identity);
            newAura.GetComponent<AuraFx>().SetupAura(buffDuration);
        }
        else if (currentScore == 5)
        {
            player.stats.IncreaseStatBy(buffPercent, buffDuration, player.stats.evasion);
            GameObject newAura = Instantiate(auraBuffEvasionPrefab, player.transform.position, Quaternion.identity);
            newAura.GetComponent<AuraFx>().SetupAura(buffDuration);
        }
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBuff();
        UnlockBuff50();
        UnlockBuff60();
        UnlockBuff70();
    }

    private void UnlockBuff()
    {
        if(buffUnlockButton.unlocked)
            buffUnlocked = true;
        buffPercent = 1f;
    }

    private void UnlockBuff50()
    {
        if (buffUnlockButton.unlocked)
            buffUnlocked = true;
        buffPercent = 1.2f;
    }
    private void UnlockBuff60()
    {
        if (buffUnlockButton.unlocked)
            buffUnlocked = true;
        buffPercent = 1.4f;
    }
    private void UnlockBuff70()
    {
        if (buffUnlockButton.unlocked)
            buffUnlocked = true;
        buffPercent = 1.6f;
    }

}
