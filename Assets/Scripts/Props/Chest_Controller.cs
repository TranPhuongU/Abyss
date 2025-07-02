using UnityEngine;

public class Chest_Controller : MonoBehaviour
{
    private ItemDrop myDropSystem;
    private Animator anim;
    private bool opened;
    [SerializeField] GameObject fButton;
    private bool playerInRange = false;

    private void Start()
    {
        myDropSystem = GetComponent<ItemDrop>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && !opened)
        {
            playerInRange = true;

            fButton.SetActive(true);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerInRange = false;

            fButton.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            myDropSystem.GenerateDrop();

            AudioManager.instance.PlaySFX(45, null);
            anim.SetBool("Open", true);

            opened = true;
            playerInRange = false;
        }
    }

}
