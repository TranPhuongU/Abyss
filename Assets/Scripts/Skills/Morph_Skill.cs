using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Morph_Skill : Skill
{
    [SerializeField] UI_SkillTreeSlot morphUnlockButton;
    public bool morph {  get; private set; }

    protected override void Start()
    {
        base.Start();
        morphUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMorph);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void CheckUnlock()
    {
        UnlockMorph();
    }

    private void UnlockMorph()
    {
        if(morphUnlockButton.unlocked)
            morph = true;
    }
}
