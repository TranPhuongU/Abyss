using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomBuffFx : MonoBehaviour
{

    private TextMeshProUGUI myTextMeshPro;
    private Transform myTransform;
    private Player player;

    private float randomDuration;
    private float randomTimer ;
    private void Start()
    {
        myTextMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetupRandomBuffFx(float _randomDuration)
    {
        randomDuration = _randomDuration;
        randomTimer = _randomDuration;
    }

    private void FixedUpdate()
    {
        randomTimer -= Time.deltaTime;

        if (randomTimer > 0)
            myTextMeshPro.text = Random.Range(1, 5).ToString();
        if(randomTimer <= 0)
        {
            myTextMeshPro.text = SkillManager.instance.buff.currentScore.ToString();
            Invoke("SetFalseActive", randomDuration);
        }

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
        randomTimer = randomDuration;

        if (player != null)
            player.onFlipped -= FlipUI;
    }

    public void FlipUI() => myTransform.Rotate(0, 180, 0);
    public void SetFalseActive() => Destroy(gameObject);
}
