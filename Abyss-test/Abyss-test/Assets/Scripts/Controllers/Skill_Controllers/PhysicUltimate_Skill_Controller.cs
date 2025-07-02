using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicUltimate_Skill_Controller : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            EnemyStats _target = collision.GetComponent<EnemyStats>();

            if(_target != null)
            {
                player.stats.DoDamage(_target);
            }

            ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

            if (weaponData != null)
                weaponData.Effect(_target.transform);
        }
    }
}
