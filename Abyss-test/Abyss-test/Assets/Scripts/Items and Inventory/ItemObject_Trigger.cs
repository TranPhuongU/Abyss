using System.Collections;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private bool canBePickedUp = false;
    private float pickupDelay = 0.5f;

    private void Start()
    {
        StartCoroutine(EnablePickupAfterDelay());
    }

    private IEnumerator EnablePickupAfterDelay()
    {
        yield return new WaitForSeconds(pickupDelay);
        canBePickedUp = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBePickedUp)
            return;

        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStats>().isDead)
                return;

            Debug.Log("Picked up item");
            myItemObject.PickupItem();
        }
    }
}
