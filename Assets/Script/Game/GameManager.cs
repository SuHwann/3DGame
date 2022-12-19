using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuCam, gameCam; //게임 카메라
    [SerializeField]
    Player player; //게임 플레이어
    [SerializeField]
    GameObject bossob; //보스 
    [SerializeField]
    GameObject cinema; //시네마씬
    [SerializeField]
    int stage;       //현재 스테이지 변수
    [SerializeField]
    float playTime;  //현재 플레이 시간
    [SerializeField]
    bool isBattle;      //전투 상황인지 아닌지 판단
    [SerializeField]
    GameObject monsterGroup; //몬스터 그룹
    [SerializeField]
    GameObject[] monsters; //일반 몬스터 배열
    public GameObject menuPanel,gamePanel,overPanel;
    public Text stageTxt,playTimeTxt,playerHealthTxt,playerAmmoTxt,playerCoinTxt;
    public RectTransform bossHealthGroup,bossHealthBar;
    [SerializeField]
    GameObject itemShop;     //아이템 샵
    [SerializeField]
    GameObject weaponShop;   //무기 샵
    public static Action DieCount; //일반 몬스터 사망시 이벤트 함수
    private int diecount =0;   //일반 몬스터 사망 갯수 카운트
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
            playTime += Time.deltaTime; //배틀 시작 순간부터 시간 계산.
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
        //현재 스테이지 UI
        stageTxt.text = "STAGE" + stage;
        //시간 계산
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime  % 60);
        //Player 플레이 시간
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
        //보스가 나오면 보스 UI출현
        if(bossob.activeSelf)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30f;
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200f;
        }
    }
    void BossCondition() //보스가 나오는 상황
    {
        diecount++;
        if(monsters.Length == diecount)
        {
            print("일반 몬스터 전부 사망 보스 출현");
            bossob.SetActive(true);
            cinema.SetActive(true); 
        }
    }
}
