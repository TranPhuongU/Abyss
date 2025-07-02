using UnityEngine;
using UnityEngine.Playables;

public class CanGat_Controller : MonoBehaviour
{
    private Animator anim;
    private bool opened;
    private Player player;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] GameObject f;

    private bool playerInRange = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = PlayerManager.instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !opened)
        {
            playerInRange = true;
            f.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerInRange = false;
            f.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("DaGat", true);

            playableDirector.Play();

            opened = true;
            playerInRange = false;
        }
    }

    public void DisableSignal()
    {
        player.enabled = false;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // dừng di chuyển nếu đang chạy
    }

    public void EnableSignal()
    {
        player.enabled = true;
    }
}
