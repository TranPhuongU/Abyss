using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isDefaultCheckpoint;

    private Animator anim;
    public string id;
    public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Awake()
    {
        if (string.IsNullOrEmpty(id))
            id = System.Guid.NewGuid().ToString();
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !activationStatus)
        {
            ActivateCheckpoint();

            // ✅ Chặn save nếu đang load scene
            if (!GameManager.instance.isLoadingGame)
            {
                GameManager.instance.SetClosestCheckpoint(this);
                SaveManager.instance.SaveGame();
            }
        }
    }

    public void ActivateCheckpoint()
    {
        if (activationStatus) return; // đã được kích hoạt trước đó → bỏ qua

        if (activationStatus == false)
            AudioManager.instance.PlaySFX(4, transform);

        activationStatus = true;

        if (anim != null)
            anim.SetTrigger("active");
    }
}
