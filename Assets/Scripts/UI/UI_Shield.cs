using UnityEngine;
using UnityEngine.UI;

public class UI_Shield : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;


    private void Start()
    {
        UpdateShieldUI();
        gameObject.SetActive(false);
        myStats.onHealthChanged += ShowHealthBarOnce;
    }

    private void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();
        entity = GetComponentInParent<Entity>();
    }

    private void Update()
    {
       
    }
    private void UpdateShieldUI()
    {
        slider.maxValue = ((EnemyStats)myStats).maxShield;
        slider.value = ((EnemyStats)myStats).currentShield;
    }

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        ((EnemyStats)myStats).onShieldChanged += UpdateShieldUI;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
            ((EnemyStats)myStats).onShieldChanged -= UpdateShieldUI;

        myStats.onHealthChanged -= ShowHealthBarOnce;
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
