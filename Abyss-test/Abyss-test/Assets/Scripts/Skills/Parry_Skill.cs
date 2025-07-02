using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{

    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")] 
    [SerializeField] private UI_SkillTreeSlot restoreForm1UnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentageForm1;
    public bool restoreForm1Unlocked { get; private set; }

    [Header("Parry restore 2")]
    [SerializeField] private UI_SkillTreeSlot restoreForm2UnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentageForm2;
    public bool restoreForm2Unlocked {  get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }

    [Header("Parry with a explode")]
    [SerializeField] UI_SkillTreeSlot parryWithExplodeUnlockButton;
    public bool parryWithExplode {  get; private set; }


    private GameObject currentParryFX;
    public float damageRate;

    [SerializeField] GameObject parryPrefabF1;
    [SerializeField] GameObject parryPrefabF2;

    public override void UseSkill()
    {
        base.UseSkill();


        if (restoreForm1Unlocked && !player.isForm2)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentageForm1);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
        else if (restoreForm2Unlocked && player.isForm2)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentageForm2);
            player.stats.IncreaseHealthBy(restoreAmount);
        }

    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreForm1UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        restoreForm2UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore2);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
        parryWithExplodeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithExplode);

    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryRestore2();
        UnlockParryWithMirage();
        UnlockParryWithExplode();
    }
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if (restoreForm1UnlockButton.unlocked)
            restoreForm1Unlocked = true;
    }

    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    private void UnlockParryWithExplode()
    {
        if(parryWithExplodeUnlockButton.unlocked)
            parryWithExplode = true;
    }

    private void UnlockParryRestore2()
    {
        if(restoreForm2UnlockButton.unlocked)
            restoreForm2Unlocked = true;
    }


    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }

    public void CreateParryFX()
    {
        if (player.isForm2)
        {
            currentParryFX = Instantiate(parryPrefabF1, new Vector2(player.transform.position.x, player.transform.position.y + .15f), Quaternion.identity);
        }
        else
            currentParryFX = Instantiate(parryPrefabF2, new Vector2(player.transform.position.x, player.transform.position.y + .15f), Quaternion.identity);
    }

    public void SetTriggerAnim()
    {
        if(parryWithExplode)
            currentParryFX.GetComponent<ParryExplode_Skill_Controller>().SetTriggerAnim();
    }

    public void DestroyParryFX()
    {
        if(currentParryFX != null)
        {
            Destroy(currentParryFX);
            currentParryFX = null;
        }
    }
    

}
