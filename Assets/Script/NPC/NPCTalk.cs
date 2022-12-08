using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup;      //UI ��ġ
    Player enterPlayer;        //player����
    public void Enter(Player player) //UI ���οø���
    { 
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector2.up;
        print("�ö󰡳�?");
    }
    public void Exit()              //UI �ٽ� �ݱ�
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
}
