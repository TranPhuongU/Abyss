using System;
using System.Collections;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;


    [Header("Fx")]
    public GameObject timeMarkFxPrefab;
    private GameObject currentSword;
    public GameObject teleportFx;
    public GameObject teleportExplosionPrefab;

    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockButton;
    [SerializeField] float damagePercent;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;
    [SerializeField] float durationTimeMark;
    public float damageReturnPercent;

    [Header("Peirce info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private bool isTeleporting;
    [SerializeField] private float explosionDamageRate;

    [Header("Time Mark skill")]
    [SerializeField] UI_SkillTreeSlot timeMarkUnlockButton;
    public bool timeMarkUnlocked {  get; private set; }

    [Header("Teleport skill")]
    [SerializeField] UI_SkillTreeSlot teleportUnlockButton;
    public bool teleportUnlocked { get; private set;}

    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;


    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetwenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenereateDots();
        SetupGravity();


        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        teleportUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTeleport);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        timeMarkUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeMark);

        //spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
       // timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        //vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnurable);

    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.E))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);


        if (Input.GetKey(KeyCode.E))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBeetwenDots);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !player.HasNoSword() && swordType == SwordType.Pierce && !isTeleporting)
        {
            if (teleportUnlocked)
            {
                Vector3 flameTime = currentSword.transform.position;
                Instantiate(teleportFx, player.transform.position, Quaternion.identity);
                StartCoroutine(TeleportWithFade(flameTime));

                Destroy(currentSword);
            }
        }
    }

    public IEnumerator TeleportWithFade(Vector3 _flameTime)
    {
        isTeleporting = true;

        // Fade out
        while (player.sr.color.a > 0)
        {
            Color c = player.sr.color;
            c.a -= fadeSpeed * Time.deltaTime;
            player.sr.color = c;
            yield return null;
        }

        // Teleport
        player.transform.position = _flameTime;
        GameObject teleportEP = Instantiate(teleportExplosionPrefab, _flameTime, Quaternion.identity);
        teleportEP.GetComponent<TeleportExplosion_Skill_Controller>().SetupTeleportExplosion(explosionDamageRate);

        // Fade in
        while (player.sr.color.a < 1)
        {
            Color c = player.sr.color;
            c.a += fadeSpeed * Time.deltaTime;
            player.sr.color = c;
            yield return null;
        }

        Destroy(currentSword);
        isTeleporting = false;
    }
    public void CreateSword()
    {
        currentSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = currentSword.GetComponent<Sword_Skill_Controller>();


        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount,bounceSpeed,durationTimeMark);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration,hitCooldown);


        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, damagePercent);

        player.AssignNewSword(currentSword);

        DotsActive(false);
    }


    #region Unlock region

    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounceSword();
        UnlockTeleport();
        UnlockPierceSword();
        UnlockTimeMark();
       // UnlockSpinSword();
        //UnlockTimeStop();
        //UnlockVulnurable();
    }
    private void UnlockSword()
    {
        if (swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockBounceSword()
    {
        if (bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }

    private void UnlockPierceSword()
    {
        if (pierceUnlockButton.unlocked)
            swordType = SwordType.Pierce;
    }

   

    private void UnlockTeleport()
    {
        if(teleportUnlockButton.unlocked)
            teleportUnlocked = true;
    }

    private void UnlockTimeMark()
    {
        if (timeMarkUnlockButton.unlocked)
            timeMarkUnlocked = true;
    }



    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVulnurable()
    {
        if (vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }
    private void UnlockSpinSword()
    {
        if (spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }
    #endregion
    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenereateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

    #endregion
}
