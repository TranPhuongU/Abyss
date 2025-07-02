using UnityEngine;

public class TimeLine : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

}
