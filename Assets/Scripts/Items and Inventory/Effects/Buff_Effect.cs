using UnityEngine;


[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]

public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffPercent;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffPercent, buffDuration, stats.GetStat(buffType));
    }
}
