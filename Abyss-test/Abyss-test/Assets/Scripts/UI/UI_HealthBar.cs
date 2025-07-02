using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    private void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();
        entity = GetComponentInParent<Entity>();
    }

    private void Start()
    {
        UpdateHealthUI();

        // Nếu không phải player, ẩn HealthBar ban đầu và chờ khi máu giảm
        if (entity.gameObject.GetComponent<Player>() == null)
        {
            gameObject.SetActive(false);
            myStats.onHealthChanged += ShowHealthBarOnce;
        }
    }

    private void OnEnable()
    {
        if (entity != null)
            entity.onFlipped += FlipUI;

        if (myStats != null)
            myStats.onHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
        {
            myStats.onHealthChanged -= UpdateHealthUI;
            myStats.onHealthChanged -= ShowHealthBarOnce;
        }
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void FlipUI()
    {
        if (myTransform != null)
            myTransform.Rotate(0, 180, 0);
    }

    private void ShowHealthBarOnce()
    {
        if (myStats.currentHealth < myStats.GetMaxHealthValue())
        {
            gameObject.SetActive(true);
            myStats.onHealthChanged -= ShowHealthBarOnce;
        }
    }
}
