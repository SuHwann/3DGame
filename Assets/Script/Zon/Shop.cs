using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup;      //UI ��ġ
    [SerializeField]
    GameObject[] itemObj;       
    Player enterPlayer;        //player����
    [SerializeField]
    int[] itemPrice;            //Item ���� �迭
    [SerializeField]    
    Text talkText;              //NPC ��ȭ
    [SerializeField]
    int health, attack, ammo;   //���� ��ǰ �ɷ�ġ;
    public string[] talkData;     //NPC ���
    public void Enter(Player player) //UI ���οø���
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector2.zero;
    }
    public void Exit()              //UI �ٽ� �ݱ�
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
    public void Buy(int index)               //���� ���
    {
        int price = itemPrice[index];
        if(price > enterPlayer.coin)    //���࿡ ���̾��ٸ�
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }
        enterPlayer.coin -= price;
        switch (index)
        { 
            case 0:
                enterPlayer.health += health;
                break;
            case 1:
                enterPlayer.attack += attack;
                Weapon.Damage(attack); //Weapon �Լ��� Damage �Լ��� ������Ѽ� �������� ���� ��Ų��.
                break;
            case 2:
                enterPlayer.ammo += ammo;
                break;
        }
    }
    //NPC ��� ���
    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
