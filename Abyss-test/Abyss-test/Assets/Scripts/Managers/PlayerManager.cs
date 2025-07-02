using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour , ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int currency;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currency = currency - _price;
        return true;
    }

    public int GetCurrency() => currency;

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;

        player.stats.strength.SetDefaultValue(_data.strengthBase);
        player.stats.agility.SetDefaultValue(_data.agilityBase);
        player.stats.intelligence.SetDefaultValue(_data.intelligenceBase);
        player.stats.vitality.SetDefaultValue(_data.vitalityBase);
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;

        _data.strengthBase = player.stats.strength.baseValue;
        _data.agilityBase = player.stats.agility.baseValue;
        _data.intelligenceBase = player.stats.intelligence.baseValue;
        _data.vitalityBase = player.stats.vitality.baseValue;
    }
}
