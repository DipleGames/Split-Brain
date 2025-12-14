using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : SingleTon<LobbyManager>
{
    [SerializeField] GameObject[] MiniGameCards;

    public void ChangeMiniGame(int targetPage)
    {
        foreach (var card in MiniGameCards)
        {
            card.SetActive(false);
        }
        MiniGameCards[targetPage].SetActive(true);
    }
}


