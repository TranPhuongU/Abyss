using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rewind_Skill : Skill
{
    
    public List<RewindData> positionHistory = new List<RewindData>();
    public float rewindDuration = 3f;
    public float recordTime = 5f;
    public float recordFrequency = 0.02f;
    private float recordTimer;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    public float rewindClockTimer;

    [SerializeField] UI_SkillTreeSlot rewindUnlockButton;
    public bool rewindUnlocked {  get; private set; }

    public GameObject rewindClockPrefab;

    protected override void Start()
    {
        base.Start();

        rewindUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockRewind);
    }

    protected override void Update()
    {
        base.Update();

        Rewind();
    }

    public void Rewind()
    {
        if (!player.stateMachine.currentState.Equals(player.rewindState))
        {
            recordTimer += Time.deltaTime;

            if (recordTimer >= recordFrequency)
            {
                positionHistory.Add(new RewindData(
                    player.transform.position,                                            // pos
                    player.transform.eulerAngles.y,                                      // rotY (quay theo Y)
                    player.anim.GetCurrentAnimatorStateInfo(0).shortNameHash,            // animHash
                    player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime,           // animTime
                    Time.time,                                               // timestamp
                    player.stats.currentHealth
                ));

                // Xoá dữ liệu cũ
                while (positionHistory.Count > 0 && Time.time - positionHistory[0].timestamp > recordTime)
                {
                    positionHistory.RemoveAt(0);
                }

                recordTimer = 0f;
            }
        }
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public void CreateClockKurumi()
    {
        base.UseSkill();
        // Tính vị trí giữa màn hình
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);

        // Với game 2D, camera thường có z = -10, nên cần set lại z của worldPosition
        worldPosition.z = 0f;

        GameObject newRewindClock = Instantiate(rewindClockPrefab, worldPosition, Quaternion.identity);

        newRewindClock.GetComponent<Rewind_Skill_Controller>().SetUpRewindClock(maxSize, growSpeed, shrinkSpeed,rewindClockTimer);


    }

    protected override void CheckUnlock()
    {
        UnlockRewind();
    }

    private void UnlockRewind()
    {
        if(rewindUnlockButton.unlocked)
            rewindUnlocked = true;
    }
}
