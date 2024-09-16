using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinPanel : NetworkBehaviour
{
    [SerializeField]
    TextMeshProUGUI victoryText;
    public override void Spawned()
    {
        base.Spawned();
        transform.parent=Singleton<GameNetworkRunner>.Instance.transform;
        gameObject.SetActive(false);
    }
    public void SetActiveWinPanel(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public void SetVictoryText(int playerId)
    {
        victoryText.text= $"Victory Player is: {playerId}";
    }
}
