using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup; //UI 위치
    [SerializeField]
    GameObject[] jewel;   //보석 
    [SerializeField]
    Text talkText;      //NPC 대화창
    public string[] talkData; //NPC 대사
    Player enterPlayer; //플레이어
    Sound speaker; //사운드
    private void Awake()
    {
        speaker = FindObjectOfType<Sound>();
    }
    public void Enter(Player player) //UI 위로 올리기 
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector2.zero;
        speaker.SoundByNum2(0);
    }
    public void Exit() //다시 닫기 
    {
        uiGroup.anchoredPosition = Vector2.down * 1000;
    }
    public void WeaponBuy(int value) //무기 사기
    {
        if (enterPlayer.jewel[value] == false) //가지고 있는 보석이없다면
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            speaker.SoundByNum2(10);
            return;
        }
        switch (value) //몇번째 무기를 교환할것인가?
        {
            case 0: //GreenSword
                if (enterPlayer.jewel[0] == true)
                { Player.SwordSwap(0); talkText.text = talkData[2]; }
                break;
            case 1: //RedSword
                if (enterPlayer.jewel[1] == true)
                { Player.SwordSwap(1); talkText.text = talkData[2]; }
                break;
            case 2: //BlueSword
                if (enterPlayer.jewel[2] == true)
                { Player.SwordSwap(2); talkText.text = talkData[2]; }
                break;
        }
        speaker.SoundByNum2(1);
    }
    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
