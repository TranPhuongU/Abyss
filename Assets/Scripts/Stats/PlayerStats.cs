using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Cinemachine.DocumentationSortingAttribute;

public class PlayerStats : CharacterStats
{
    private Player player;
    private int level = 1;
    public int currentExp;
    public int maxExp;
    public int expIncreasePerLevel;

    public int attributePoints;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;

    protected override void Start()
    {
        base.Start();

        player= GetComponent<Player>();
        expIncreasePerLevel = Mathf.RoundToInt( maxExp * .5f);

        attributePoints = 10;
       
    }

    protected override void Update()
    {
        base.Update();


        if(currentExp >= maxExp)
        {
            ApplyLevelModifiers();

            level++;

            currentExp = 0;
            expIncreasePerLevel += Mathf.RoundToInt(maxExp * .3f);
            maxExp += expIncreasePerLevel;

            fx.CreatePopUpText("Level Up", Color.pink);

            attributePoints += 10;

        }
    }

    public void GainExp(int amount)
    {
        currentExp += amount;
        // Update UI hoặc xử lý lên cấp nếu cần
    }

    private void ApplyLevelModifiers()
    {
        //Modify(strength);
        //Modify(agility);
        //Modify(intelligence);
        //Modify(vitality);

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

    }

    private void Modify(Stat _stat)
    {
        float modifier = _stat.GetValue() * percentageModifier;
        _stat.AddModifier(Mathf.RoundToInt(modifier));

    }

    protected override void Die()
    {  
        player.Die();

        if (player.isForm2 && player.skill.rewind.cooldownTimer <= 0)
        {
            if(player.skill.rewind.rewindUnlocked)
                return;
        }

        base.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage, Color _color)
    {
        base.DecreaseHealthBy(_damage, _color);

        if(isDead)
            return;

        if(_damage > GetMaxHealthValue() * 0.3f)
        {
            player.SetupKnockbackPower(new Vector2(10, 6));
           // player.fx.ScreenShake(player.fx.shakeHighDamage);

            //aam thanh khi bi knockback
            //int randomSound = Random.Range(34, 35);
            //AudioManager.instance.PlaySFX(randomSound, null);
        }
         
        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats,float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        //_targetStats.TakeDamage(totalDamage, Color.white);


        DoMagicalDamage(_targetStats); // remove if you don't want to apply magic hit on primary attack
    }
    public virtual void TakeBurnDamage(int _damage, Color _color)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(_damage, _color);
        if (_damage > 0 && !isIgnited)
            fx.CreatePopUpText(_damage.ToString(), _color);

        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
            Die();

    }
}
