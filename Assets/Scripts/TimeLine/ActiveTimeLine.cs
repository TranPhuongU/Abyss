using UnityEngine;
using UnityEngine.Playables;

public class ActiveTimeLine : MonoBehaviour
{
    private bool opened;
    private Player player;
    [SerializeField] private PlayableDirector playableDirector;
    

    private bool playerInRange = false;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !opened)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            playableDirector.Play();

            player.rb.linearVelocity = Vector2.zero;

            opened = true;
            playerInRange = false;
        }
    }

    public void DisableSignal()
    {
        player.enabled = false;
    }

    public void EnableSignal()
    {
        player.enabled = true;
    }


}
