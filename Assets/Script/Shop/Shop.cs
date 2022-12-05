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
    Player enterPlayer;
    [SerializeField]
    int[] itemPrice;            //Item ���� �迭
    [SerializeField]
    Transform[] itemPos;        //Item ������ġ
    [SerializeField]
    Text talkText;
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
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3)
            +Vector3.forward * Random.Range(-3, 3);
        //���Լ����� ������ ����
        Instantiate(itemObj[index], itemPos[index].position + ranVec, itemPos[index].rotation);
        itemObj[index].transform.localScale = new Vector3(4, 4, 4);
    }
    //NPC ��� ���
    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
