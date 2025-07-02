using UnityEngine;

public class EnemyStats : CharacterStats
{

    [SerializeField] private GameObject expOrbPrefab;
    [SerializeField] private int expDropAmount = 5;

    private Enemy enemy;
    private Player player;
    private ItemDrop myDropSystem;
    public Stat soulsDropAmount;

    public float maxShield;
    public float currentShield;

    [SerializeField] private int expAmount;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;

    private TimeMarkTracker timeMarkTracker;

    public System.Action onShieldChanged;

    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100);
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        player = PlayerManager.instance.player;
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void Awake()
    {
        maxShield = Mathf.RoundToInt(maxHealth.GetValue() * .4f);
        currentShield = maxShield;
    }

    protected override void Update()
    {
        base.Update();

        if (timeMarkTracker != null)
        {
            timeMarkTracker.Tick(Time.deltaTime);

            if (timeMarkTracker.IsExpired())
            {
                //gay no voi 10% sat thuong da nhan khi có ấn thời gian
                int explosionDamage = Mathf.RoundToInt(timeMarkTracker.totalDamage * 0.1f);

                TakeDamage(explosionDamage, Color.cyan);

                timeMarkTracker = null;
            }
        }
    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    protected override void DecreaseHealthBy(int _damage, Color _color)
    {
        base.DecreaseHealthBy(_damage, _color);

        if (timeMarkTracker != null)
        {
            timeMarkTracker.AddDamage(_damage);
        }
    }

    protected override void Die()
    {
        base.Die();

        isDead = true;

        enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.GetValue();

        if (isChilled)
        {

            if (player.onIceEnergyChanged != null)
                player.onIceEnergyChanged();
        }

        myDropSystem.GenerateDrop();

        //((PlayerStats)player.stats).currentExp += expAmount;

        SpawnExpOrbs();
        Destroy(gameObject, 5f);

    }

    private void SpawnExpOrbs()
    {
        for (int i = 0; i < expDropAmount; i++)
        {
            Instantiate(expOrbPrefab, transform.position, Quaternion.identity);
        }
    }

    public void StartTimeMark(float _duration, float _timeMarkFXDuration)
    {
        if (timeMarkTracker == null)
        {
            timeMarkTracker = new TimeMarkTracker(_duration);

            GameObject newTimeMarkUI = Instantiate(SkillManager.instance.sword.timeMarkFxPrefab, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 3), Quaternion.identity);

            newTimeMarkUI.GetComponent<TimeMarkFX>().SetupTimeMarkFX(enemy, _timeMarkFXDuration);
        }
    }

    public void DoBurnDamageRate(PlayerStats _targetStats, float _rate)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();


        int totalMagicalDamage = Mathf.RoundToInt((_fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue()) * _rate);

        fx.CreateHitFx(_targetStats.transform, false);

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeBurnDamage(totalMagicalDamage, Color.magenta);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;


        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

    }

    public override void TakeDamage(int _damage, Color _color)
    {
        base.TakeDamage(_damage, _color);

        if (!enemy.canShield)
            return;

        currentShield -= _damage;

        if (onShieldChanged != null)
            onShieldChanged();
    }


}
