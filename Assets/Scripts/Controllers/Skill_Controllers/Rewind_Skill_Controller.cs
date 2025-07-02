using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind_Skill_Controller : MonoBehaviour
{
    private bool canGrow = true;
    private bool canShrink;
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float rewindClockTimer;
    void Start()
    {
        
    }

    public void SetUpRewindClock(float _maxSize, float _growSpeed, float shrinkSpeed, float _rewindClockTimer)
    {
        this.maxSize = _maxSize;
        this.growSpeed = _growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        rewindClockTimer = _rewindClockTimer;
    }

    void Update()
    {
        rewindClockTimer -= Time.deltaTime;

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (rewindClockTimer <=0)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
        
    }
}
