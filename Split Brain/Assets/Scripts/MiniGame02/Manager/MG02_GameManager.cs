using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG02_GameManager : SingleTon<MG02_GameManager>
{
    public void GameOver()
    {
        Debug.Log("게임 종료");
    }
}
