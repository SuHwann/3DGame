using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup;      //UI 위치
    Player enterPlayer;        //player변수
    public void Enter(Player player) //UI 위로올리기
    { 
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector2.up;
        print("올라가나?");
    }
    public void Exit()              //UI 다시 닫기
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
}
