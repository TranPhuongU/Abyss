using UnityEngine;

public class IncreaseAttributeButton : MonoBehaviour
{
    [SerializeField] private StatType statType;

    private void Start()
    {
        
    }
    public void IncreaseAttribute()
    {
        if (((PlayerStats)PlayerManager.instance.player.stats).attributePoints <= 0)
            return;

        if(statType == StatType.strength)
        {
            PlayerManager.instance.player.stats.strength.baseValue++;
        }

        if (statType == StatType.intelegence)
        {
            PlayerManager.instance.player.stats.intelligence.baseValue++;
        }

        if (statType == StatType.agility)
        {
            PlayerManager.instance.player.stats.agility.baseValue++;
        }

        if (statType == StatType.vitality)
        {
            PlayerManager.instance.player.stats.vitality.baseValue++;
        }

        ((PlayerStats)PlayerManager.instance.player.stats).attributePoints--;
    }
}
