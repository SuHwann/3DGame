using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup; //UI ��ġ
    [SerializeField]
    GameObject[] jewel;   //���� 
    [SerializeField]
    Text talkText;      //NPC ��ȭâ
    public string[] talkData; //NPC ���
    Player enterPlayer; //�÷��̾�
    Sound speaker; //����
    private void Awake()
    {
        speaker = FindObjectOfType<Sound>();
    }
    public void Enter(Player player) //UI ���� �ø��� 
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector2.zero;
        speaker.SoundByNum2(0);
    }
    public void Exit() //�ٽ� �ݱ� 
    {
        uiGroup.anchoredPosition = Vector2.down * 1000;
    }
    public void WeaponBuy(int value) //���� ���
    {
        if (enterPlayer.jewel[value] == false) //������ �ִ� �����̾��ٸ�
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            speaker.SoundByNum2(10);
            return;
        }
        switch (value) //���° ���⸦ ��ȯ�Ұ��ΰ�?
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
