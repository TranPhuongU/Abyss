using UnityEngine;
using System.Collections;
using System.Timers;

public class Saw_Controller : MonoBehaviour
{
    [SerializeField] private Transform begin;
    [SerializeField] private Transform over;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private int damage = 10;

    private LineRenderer line;

    private Vector3 target;

    private void Start()
    {
        target = over.position;
        StartCoroutine(MoveSaw());

        line = GetComponent<LineRenderer>();
        if (line != null)
        {
            line.positionCount = 2;
            line.SetPosition(0, begin.position);
            line.SetPosition(1, over.position);
        }

    }

    private void Update()
    {
    }
    private IEnumerator MoveSaw()
    {
        while (true)
        {
            // Di chuyển tới target
            while (Vector2.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Chờ 1s
            yield return new WaitForSeconds(waitTime);

            // Đảo hướng
            target = (target == over.position) ? begin.position : over.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerStats>(out var playerStats))
        {
            collision.GetComponent<Player>().SetupKnockbackDir(transform);

            collision.GetComponent<Player>().SetupKnockbackPower(new Vector2(10, 6));
           
            playerStats.TakeDamage(damage, Color.white);

            
        }
    }
}
