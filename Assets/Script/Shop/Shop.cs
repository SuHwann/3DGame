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
    Player enterPlayer;
    [SerializeField]
    int[] itemPrice;            //Item 가격 배열
    [SerializeField]
    Transform[] itemPos;        //Item 생성위치
    [SerializeField]
    Text talkText;
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
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3)
            +Vector3.forward * Random.Range(-3, 3);
        //구입성공시 아이템 생성
        Instantiate(itemObj[index], itemPos[index].position + ranVec, itemPos[index].rotation);
        itemObj[index].transform.localScale = new Vector3(4, 4, 4);
    }
    //NPC 대사 기능
    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
