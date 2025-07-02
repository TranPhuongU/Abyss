using UnityEngine;
using UnityEngine.Rendering.Universal; // Nhớ import để dùng Light2D

[RequireComponent(typeof(Light2D))]
public class FireEffect : MonoBehaviour
{
    private Light2D fireLight;

    [Header("Cường độ ánh sáng")]
    public float intensityMin = 0.8f;
    public float intensityMax = 1.2f;

    [Header("Tốc độ dao động")]
    public float flickerSpeed = 2f;

    [Header("Biên độ dao động ngẫu nhiên")]
    public float randomStrength = 0.1f;

    private float baseIntensity;

    void Awake()
    {
        fireLight = GetComponent<Light2D>();
        baseIntensity = fireLight.intensity;
    }

    void Update()
    {
        // Tạo hiệu ứng nhấp nháy bằng sóng sin + ngẫu nhiên
        float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
        float randomOffset = Random.Range(-randomStrength, randomStrength);
        float targetIntensity = Mathf.Lerp(intensityMin, intensityMax, flicker) + randomOffset;

        fireLight.intensity = Mathf.Clamp(targetIntensity, intensityMin, intensityMax);
    }
}
