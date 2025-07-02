using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AuraFx : MonoBehaviour
{
    private Transform myTransform;

    private Player player;

    private float timer;


    public void SetupAura(float _timer)
    {
        timer = _timer;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Destroy(gameObject);
    }

    private void OnEnable()
    {

        myTransform = GetComponent<Transform>();
        transform.parent = PlayerManager.instance.player.transform;
        player = GetComponentInParent<Player>();
        player.onFlipped += FlipUI;
    }



    private void OnDisable()
    {
        if (player != null)
            player.onFlipped -= FlipUI;
    }

    public void FlipUI() => myTransform.Rotate(0, 180, 0);
}
