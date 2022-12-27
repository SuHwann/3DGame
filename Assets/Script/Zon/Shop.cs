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
    Text talkText;              //NPC 대화
    [SerializeField]
    int health, attack, ammo;   //상점 상품 능력치;
    public string[] talkData;     //NPC 대사
    Sound speaker; //스피커 변수
    private void Awake()
    {
        speaker = FindObjectOfType<Sound>();
    }
    public void Enter(Player player) //UI 위로올리기
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector2.zero;
        speaker.SoundByNum2(0);
    }
    public void Exit()              //UI 다시 닫기
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }
    public void Buy(int index)               //구입 기능
    {
        int price = itemPrice[index];
        if(price > Player.coin)    //만약에 돈이없다면
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            speaker.SoundByNum2(10);
            return;
        }
        Player.coin -= price;
        switch (index)
        { 
            case 0:
                Player.health += health; //체력 증가
                break;
            case 1:
                Player.attack += attack; //공격력 증가
                Weapon.Damage(attack); //Weapon 함수의 Damage 함수를 실행시켜서 데미지를 증가 시킨다.
                break;
            case 2:
                Player.ammo += ammo; //방어력 증가
                break;
        }
        speaker.SoundByNum2(1);
    }
    //NPC 대사 기능
    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
