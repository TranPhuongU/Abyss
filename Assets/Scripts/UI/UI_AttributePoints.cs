using TMPro;
using UnityEngine;

public class UI_AttributePoints : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attributePointsText;

    private void Start()
    {
        attributePointsText.text = ((PlayerStats)PlayerManager.instance.player.stats).attributePoints.ToString();
    }

    private void Update()
    {
        attributePointsText.text = ((PlayerStats)PlayerManager.instance.player.stats).attributePoints.ToString();
    }
}
