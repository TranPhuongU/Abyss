using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMarkTracker 
{
    public float duration;
    public float timer;
    public int totalDamage;

    public TimeMarkTracker(float _duration)
    {
        duration = _duration;
        timer = _duration;
        totalDamage = 0;
    }

    public void AddDamage(int _amnount)
    {
        totalDamage += _amnount;
    }

    public bool IsExpired()
    {
        return timer <= 0;
    }

    public void Tick(float _deltaTime)
    {
        timer -= _deltaTime;
    }
}
