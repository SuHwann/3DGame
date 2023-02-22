using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup;      //UI ��ġ
    [SerializeField]        
    Text text;                  //text ����
    [SerializeField]
    string[] npctext;            //talk �迭
    int talkIndex;             //talk �迭 index
    [SerializeField]
    int jewelNum; //���° ������ �ٰ��ΰ� ����
    bool talkline = true; //NPC ��簡 �������� �Ǵ�
    public void Enter(Player player) //UI ���οø���
    {
        if(talkline)
        {
            uiGroup.anchoredPosition = Vector2.zero;
            StartCoroutine(Talk());
        }
    }
    public void Exit()              //UI �ٽ� �ݱ�
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
    IEnumerator Talk()
    {
        text.text = " "; //text �ʱ�ȭ 
        talkline = false;
        if (talkIndex == npctext.Length) //npctext �迭�� ���̿� ���� ��µǰ��ִ� �迭 ������ ������ ��ȭ�� �����ٰ� �Ǵ�
        {
            talkIndex = 0;
            yield break;
        }
        foreach (char letter in npctext[talkIndex++]) //���ڸ� �ɰ��� �ϳ��� ��� 
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
            if (npctext.Length == talkIndex) //���࿡ ��ȭ�� �����ٸ�
            {
                Player.GiveJewel(jewelNum);
            }
            else StartCoroutine(Talk());
        }
    }
}
