using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    [SerializeField]
    RectTransform uiGroup;      //UI 위치
    [SerializeField]
    GameObject[] itemObj;       
    Player enterPlayer;        //player변수
    [SerializeField]
    int[] itemPrice;            //Item 가격 배열
    [SerializeField]
    Transform[] itemPos;        //Item 생성위치
    [SerializeField]
    Text talkText;
    [SerializeField]
    int health, attack, ammo;   //상점 상품 능력치;
    public string[] talkData;     //NPC 대사
    public void Enter(Player player) //UI 위로올리기
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector2.zero;
    }
    public void Exit()              //UI 다시 닫기
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
    public void Buy(int index)               //구입 기능
    {
        int price = itemPrice[index];
        if(price > enterPlayer.coin)    //만약에 돈이없다면
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
                print("체력이 증가했다 : " + enterPlayer.health);
                break;
            case 1:
                enterPlayer.attack += attack;
                Weapon.Damage(attack); //Weapon 함수의 Damage 함수를 실행시켜서 데미지를 증가 시킨다.
                print("공격력이 증가했다"+ enterPlayer.attack);
                break;
            case 2:
                enterPlayer.ammo += ammo;
                print("방어력이 증가했다 : "+ enterPlayer.ammo);
                break;
        }
    }
    //NPC 대사 기능
    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
