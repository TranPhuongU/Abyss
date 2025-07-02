using UnityEngine;

public class RewindData
{
    public Vector3 position;
    public float rotationY;
    public int animStateHash;
    public float animNormalizedTime;
    public float timestamp;
    public int currentHealth;

    public RewindData(Vector3 pos, float rotY, int animHash, float animTime, float time, int _currentHealth)
    {
        position = pos;
        rotationY = rotY;
        animStateHash = animHash;
        animNormalizedTime = animTime;
        timestamp = time;
        currentHealth = _currentHealth;
    }
}
