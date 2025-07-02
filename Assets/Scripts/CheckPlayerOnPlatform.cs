using UnityEngine;

public class CheckPlayerOnPlatform : MonoBehaviour
{
    [SerializeField] private Enemy_Shadow enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            enemy.playerOnPlatform = true;
            enemy.hasTeleportedToPlatform = false; // Reset lại mỗi lần player lên platform
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            enemy.playerOnPlatform = false;
        }
    }

}
