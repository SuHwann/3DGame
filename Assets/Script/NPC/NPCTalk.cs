using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup;      //UI ��ġ
    [SerializeField]         //text ����
    Text text;
    [SerializeField]
    string[] npctext;            //talk �迭
    int talkIndex;             //talk �迭 index

    public void Enter(Player player) //UI ���οø���
    { 
        uiGroup.anchoredPosition = Vector2.zero;
        StartCoroutine(Talk());
    }
    public void Exit()              //UI �ٽ� �ݱ�
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
    IEnumerator Talk()
    {
        text.text = " ";
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
        if(npctext.Length == talkIndex) //���࿡ ��ȭ�� �����ٸ�
        {
            print("��ȭ ��");
        }
        else 
        StartCoroutine(Talk());
    }
}
