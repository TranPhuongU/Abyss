using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class SilentWorld_Skill : Skill
{

    [Header("Silent world")]
    [SerializeField] private UI_SkillTreeSlot silentWorldUnlockButton;
    public bool silentWorldUnlocked;// { get; private set; }
    [SerializeField] private int amountOfClones;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float silentWorldDuration;
    [Space]
    [SerializeField] private GameObject silentWorldPrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    SilentWorld_Skill_Controller currentSilentWorld;


    [Header("TimeMark")]
    [SerializeField] UI_SkillTreeSlot silentWorldWithTimeMarkUnlockButton;
    [SerializeField] float timeMarkDuration;

    public bool silentWordWithTimeMarkUnlocked { get; private set; }

    [Header("Increase Clone")]
    [SerializeField] UI_SkillTreeSlot increaseCloneUnlockButton;
    public bool increaseCloneUnlocked;
    [SerializeField] int amountOfIncreaseClone;

    

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(silentWorldPrefab,player.transform.position,Quaternion.identity);

        currentSilentWorld = newBlackHole.GetComponent<SilentWorld_Skill_Controller>();

        currentSilentWorld.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfClones, cloneCooldown,silentWorldDuration, timeMarkDuration);

        AudioManager.instance.PlaySFX(37, player.transform);
        AudioManager.instance.PlaySFX(8, player.transform);
    }

    protected override void Start()
    {
        base.Start();

        silentWorldUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSilentWorld);
        silentWorldWithTimeMarkUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSilentWorldWithTimeMark);
        increaseCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockIncreaseClone);
    }

    protected override void Update()
    {
        base.Update();
    }


    public bool SkillCompleted()
    {
        if (!currentSilentWorld)
            return false;


        if (currentSilentWorld.playerCanExitState)
        {
            currentSilentWorld = null;
            return true;
        }


        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockSilentWorld();
        UnlockSilentWorldWithTimeMark();
        UnlockIncreaseClone();
    }
    private void UnlockSilentWorld()
    {
        if (silentWorldUnlockButton.unlocked)
            silentWorldUnlocked = true;

    }

    private void UnlockSilentWorldWithTimeMark()
    {
        if(silentWorldWithTimeMarkUnlockButton.unlocked)
            silentWordWithTimeMarkUnlocked = true;
    }

    private void UnlockIncreaseClone()
    {
        if (increaseCloneUnlockButton.unlocked)
        {
            increaseCloneUnlocked = true;
            amountOfClones += amountOfIncreaseClone;
            timeMarkDuration += 3.5f;

        }
    }

    
}
