using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMarkFX : MonoBehaviour
{
    private Transform myTransform;

    private Entity entity;

    private float timeMarkFXDuration;
    private void Awake()
    {
        myTransform = GetComponent<Transform>();
    }

    public void SetupTimeMarkFX(Entity _entity, float _timeMarkFXDuration)
    {
        entity = _entity;
        entity.onFlipped += FlipUI;
        transform.parent = _entity.transform;
        timeMarkFXDuration = _timeMarkFXDuration;
    }

    private void Update()
    {
        timeMarkFXDuration -= Time.deltaTime;
        if(timeMarkFXDuration <= 0)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;
    }

    public void FlipUI() => myTransform.Rotate(0, 180, 0);
}
