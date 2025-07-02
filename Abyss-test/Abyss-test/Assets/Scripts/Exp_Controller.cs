using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    private Transform player;

    [Header("Speed Settings")]
    [SerializeField] private float launchSpeed = 4f;  // tốc độ bay ra
    [SerializeField] private float returnSpeed = 6f;  // tốc độ bay về

    private bool isReturning = false;
    private float delayBeforeReturn = 0.5f;

    private int expValue;

    private void Start()
    {
        player = PlayerManager.instance.player.transform;

        expValue = Random.Range(10, 15);

        Vector2 moveDir = Random.insideUnitCircle.normalized;
        GetComponent<Rigidbody2D>().linearVelocity = moveDir * launchSpeed;

        Invoke(nameof(StartReturn), delayBeforeReturn);
    }

    private void Update()
    {
        if (isReturning && player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.position) < 0.5f)
            {
                player.GetComponent<PlayerStats>().GainExp(expValue);
                Destroy(gameObject);
            }
        }
    }

    private void StartReturn()
    {
        isReturning = true;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}
