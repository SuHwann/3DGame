using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup;      //UI 위치
    [SerializeField]         //text 변수
    Text text;
    [SerializeField]
    string[] npctext;            //talk 배열
    int talkIndex;             //talk 배열 index
    [SerializeField]
    int jewelNum; //몇번째 보석을 줄것인가 변수
    public void Enter(Player player) //UI 위로올리기
    {
        uiGroup.anchoredPosition = Vector2.zero;
        StartCoroutine(Talk());
    }
    public void Exit()              //UI 다시 닫기
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
    IEnumerator Talk()
    {
        text.text = " ";
        if (talkIndex == npctext.Length)
        {
            talkIndex = 0;
            yield break;
        }
        foreach (char letter in npctext[talkIndex++])
        {
            text.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        yield break;
    }
    public void NEXT_Talk()
    {
        if(npctext.Length == talkIndex) //만약에 대화가 끝났다면
        {
            print("대화 끝");
            Player.GiveJewel(jewelNum);
        }
        else 
        StartCoroutine(Talk());
    }
}
