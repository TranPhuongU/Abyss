using UnityEngine;

public class BossFighting : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()  != null)
        {
            enemy.fighting = true;
        }
    }
}
