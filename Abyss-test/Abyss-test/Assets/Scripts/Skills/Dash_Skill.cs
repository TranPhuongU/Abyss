using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;

    [Header("Bomb Detail")]
    [SerializeField] GameObject bombPrefab;
    public float damagePercent;
    public float bombDuration;
    public float bombTime;
    private float bombTimer;
    public bool dashUnlocked { get; private set; }

    [Header("Slash Wave info")]
    [SerializeField] private UI_SkillTreeSlot slashWaveUnlockButton1;
    public bool slashWave1 { get; private set; }


    [SerializeField] private UI_SkillTreeSlot slashWaveUnlockButton2;
    public bool slashWave2 { get; private set; }


    [SerializeField] UI_SkillTreeSlot slashWaveUnlockButton3;
    public bool slashWave3 { get; private set; }


    [Header("Sand Boom")]
    [SerializeField] UI_SkillTreeSlot sandBombButton1;
    public bool sandBomb1 { get; private set; }

    [SerializeField] UI_SkillTreeSlot sandBombButton2;
    public bool sandBomb2 {  get; private set; }


    [SerializeField] UI_SkillTreeSlot sandBombButton3;
    public bool sandBomb3 { get; private set; }



    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        bombTimer = bombTime;

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        slashWaveUnlockButton1.GetComponent<Button>().onClick.AddListener(UnlockSlashWave1);
        slashWaveUnlockButton2.GetComponent<Button>().onClick.AddListener(UnlockSlashWave2);
        slashWaveUnlockButton3.GetComponent<Button>().onClick.AddListener(UnlockSlashWave3);
        sandBombButton1.GetComponent<Button>().onClick.AddListener(UnlockSandBomb1);
        sandBombButton2.GetComponent<Button>().onClick.AddListener(UnlockSandBomb2);
        sandBombButton3.GetComponent<Button>().onClick.AddListener(UnlockSandBomb3);
    }

    protected override void Update()
    {
        base.Update(); 

        bombTimer -= Time.deltaTime;
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockSlashWave1();
        UnlockSlashWave2();
        UnlockSlashWave3();
        UnlockSandBomb1();
        UnlockSandBomb2();
        UnlockSandBomb3();
    }

    private void UnlockDash()
    { 
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockSlashWave1()
    {
        if (slashWaveUnlockButton1.unlocked)
            slashWave1 = true;
    }

    private void UnlockSlashWave2()
    {
        if(slashWaveUnlockButton2.unlocked)
            slashWave2 = true;
    }

    private void UnlockSlashWave3()
    {
        if(slashWaveUnlockButton3.unlocked)
            slashWave3 = true;
    }

    private void UnlockSandBomb1()
    {
        if (sandBombButton1.unlocked)
        {
            sandBomb1 = true;
            bombTime = 0.1f;
        }
    }

    private void UnlockSandBomb2()
    {
        if (sandBombButton2.unlocked)
        {
            sandBomb2 = true;
            bombTime = 0.09f;
        }
    }

    private void UnlockSandBomb3()
    {
        if (sandBombButton3.unlocked)
        {
            sandBomb3 = true;
            bombTime = 0.07f;
        }
    }


    //public void CloneOnDash()
    //{
    //    if(slashWave1)
    //        SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    //}

    //public void CloneOnArrival()
    //{
    //    if(slashWave2)
    //        SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    //}

    public void CreateBomb()
    {
        if (bombTimer <= 0 && sandBomb1)
        {
            GameObject newSandBomb = Instantiate(bombPrefab, player.transform.position, Quaternion.identity);
            newSandBomb.GetComponent<SandBomb_Skill_Controller>().SetupSandBomb(player.stats, damagePercent,bombDuration);

           bombTimer = bombTime;
        }
    }
}
