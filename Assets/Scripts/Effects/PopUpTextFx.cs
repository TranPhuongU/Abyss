using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextFx : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float speed;
    [SerializeField] private float desapearanceSpeed;
    [SerializeField] private float colorDesapearanceSpeed;

    [SerializeField] private float lifeTime;

    private float textTimer;

    private void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        textTimer -= Time.deltaTime;

        if(textTimer <0)
        {
            float alpha = myText.color.a - colorDesapearanceSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a < 50)
                speed = desapearanceSpeed;

            if (myText.color.a <= 0)
                Destroy(gameObject);
        }
        
    }
}
