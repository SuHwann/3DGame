using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuCam, gameCam;
    [SerializeField]
    Player player;
    [SerializeField]
    GolemEarth boss;
    [SerializeField]
    string bossScript;
    [SerializeField]
    int stage;
    [SerializeField]
    float playTime;
    [SerializeField]
    bool isBattle;      //���� �������ΰ�
    [SerializeField]
    GameObject monsterGroup;
    [SerializeField]
    GameObject[] monsters; //�Ϲ� ���͵�
    public GameObject menuPanel,gamePanel,overPanel;
    public Text stageTxt,playTimeTxt,playerHealthTxt,playerAmmoTxt,playerCoinTxt;
    public RectTransform bossHealthGroup,bossHealthBar;
    public GameObject itemShop;     //������ ����
    public static Action DieCount;
    private int diecount =0;   //���� ��� ī��Ʈ
    private void Awake()
    {
        DieCount = () => { BossCondition(); };
    }
    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
        isBattle = true;
        monsterGroup.SetActive(true);
        itemShop.SetActive(true);
    }
    private void Update()
    {
        if(isBattle)
        {
            playTime += Time.deltaTime; //delta�� ������ �����ָ� �װ� �� �ð��� �ȴ�.
        }
    }
    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        isBattle = false;
        monsterGroup.SetActive(false); 
        itemShop.SetActive(false);  
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    private void LateUpdate()
    {
        //��� UI
        stageTxt.text = "STAGE" + stage;
        //�ð�
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime  % 60);
        //Play�ð� 
        playTimeTxt.text = string.Format("{0:00}", hour) +":"+ string.Format("{0:00}",min) + ":" + string.Format("{0:00}", second);
        //Player UI
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}",player.coin);
        if(player.equipWeapon == null)
        {playerAmmoTxt.text = "- / " + player.ammo;}
        else if(player.equipWeapon.type == Weapon.Type.Melee)
        playerAmmoTxt.text = "- /" + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " /" + player.ammo;
        //���� ���� ü��
        if(boss != null)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30f;
            bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200f;
        }
        if (boss.curHealth < 0) { bossHealthBar.localScale =  Vector3.zero; };
    }
    void BossCondition() //������ ������ ���� 
    {
        diecount++;
        if(monsters.Length == diecount)
        {
            print("��� ���� ���");
        }
    }
}
