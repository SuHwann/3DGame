using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup;      //UI 위치
    [SerializeField]        
    Text text;                  //text 변수
    [SerializeField]
    string[] npctext;            //talk 배열
    int talkIndex;             //talk 배열 index
    [SerializeField]
    int jewelNum; //몇번째 보석을 줄것인가 변수
    bool talkline = true; //NPC 대사가 끝났는지 판단
    public void Enter(Player player) //UI 위로올리기
    {
        if(talkline)
        {
            uiGroup.anchoredPosition = Vector2.zero;
            StartCoroutine(Talk());
        }
    }
    public void Exit()              //UI 다시 닫기
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
    IEnumerator Talk()
    {
        text.text = " "; //text 초기화 
        talkline = false;
        if (talkIndex == npctext.Length) //npctext 배열의 길이와 현재 출력되고있는 배열 순서가 같으면 대화가 끝났다고 판단
        {
            talkIndex = 0;
            yield break;
        }
        foreach (char letter in npctext[talkIndex++]) //글자를 쪼개서 하나씩 출력 
        {
            text.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        talkline = true;

        yield break;
    }
    public void NEXT_Talk()
    {
        if(talkline)
        {
            if (npctext.Length == talkIndex) //만약에 대화가 끝났다면
            {
                Player.GiveJewel(jewelNum);
            }
            else StartCoroutine(Talk());
        }
    }
}
