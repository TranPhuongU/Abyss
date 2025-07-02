using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int strengthBase;
    public int agilityBase;
    public int intelligenceBase;
    public int vitalityBase;

    public int currency;

    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;


    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public SerializableDictionary<string, float> volumeSettings;

    public List<string> unlockedMaps;

    public SerializableDictionary<string, SerializableDictionary<string, bool>> allMapCheckpoints;

    public SerializableDictionary<string, string> mapClosestCheckpointIds;



    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;


        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();

        volumeSettings = new SerializableDictionary<string, float>();

        unlockedMaps = new List<string> { "Map1" }; // mặc định có Map1

        allMapCheckpoints = new SerializableDictionary<string, SerializableDictionary<string, bool>>();

        mapClosestCheckpointIds = new SerializableDictionary<string, string>();



    }
}
