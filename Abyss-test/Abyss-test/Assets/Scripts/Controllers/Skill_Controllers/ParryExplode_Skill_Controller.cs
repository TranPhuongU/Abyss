using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryExplode_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    private Player player;
    private float damageRate = .2f;

    private void Start()
    {
        anim = GetComponent<Animator>();   
        player = PlayerManager.instance.player;
        cd = GetComponent<CircleCollider2D>();

        damageRate = SkillManager.instance.parry.damageRate;
    }

    public void SetTriggerAnim() => anim.SetTrigger("Explode");

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                player.stats.DoDamageRate(hit.GetComponent<CharacterStats>(),damageRate);


                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }

    private void DestroyMe() => Destroy(gameObject);
}
