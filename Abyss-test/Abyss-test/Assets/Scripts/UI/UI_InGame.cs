using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Player player;
    [SerializeField] private Slider sliderHp;
    [SerializeField] private Slider sliderEnergy;
    [SerializeField] private Slider sliderExp;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;


    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;

    void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
            player.onEnergyChanged += UpdateEnergyUI;
            
        }

        skills = SkillManager.instance;
    }

    
    void Update()
    {
        UpdateSoulsUI();
        UpdateExpUI();

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        //if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
        //    SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.E) && skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.silentWorld.silentWorldUnlocked)
            SetCooldownOf(blackholeImage);


        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        //CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        if (player.isForm2)
        {
            CheckCooldownOf(swordImage, skills.sword.cooldown);
            CheckCooldownOf(blackholeImage, skills.silentWorld.cooldown);
        }
        else
        {
            CheckCooldownOf(swordImage, skills.buff.cooldown);
            CheckCooldownOf(blackholeImage, skills.physicUltimate.cooldown);
        }
            


        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);

    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrency())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrency();


        currentSouls.text = ((int)soulsAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        sliderHp.maxValue = playerStats.GetMaxHealthValue();
        sliderHp.value = playerStats.currentHealth;
    }

    private void UpdateEnergyUI()
    {
        sliderEnergy.maxValue = player.maxDashEnergy;
        sliderEnergy.value = player.CurrentDashEnergy;
    }


    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

    private void UpdateExpUI()
    {
        sliderExp.maxValue = ((PlayerStats)player.stats).maxExp;
        sliderExp.value = ((PlayerStats)player.stats).currentExp;
    }


}
