using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportExplosion_Skill_Controller : MonoBehaviour
{
    private CircleCollider2D cd;
    private Player player;
    private float damageRate = .5f;


    private void Start()
    {
        player = PlayerManager.instance.player;
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetupTeleportExplosion(float _damageRate)
    {
        damageRate = _damageRate;
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                player.stats.DoMagicalDamageRate(hit.GetComponent<CharacterStats>(), damageRate);


                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }

    private void selfDestroy() => Destroy(gameObject);
}
