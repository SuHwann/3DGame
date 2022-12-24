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
    public GameObject menuPanel,gamePanel,overPanel,clearPanel;
    public Text stageTxt,playTimeTxt,playerHealthTxt,playerAmmoTxt,playerCoinTxt, bossText;
    public RectTransform bossHealthGroup,bossHealthBar;
    [SerializeField]
    GameObject itemShop;     //아이템 샵
    [SerializeField]
    GameObject weaponShop;   //무기 샵
    public static Action DieCount; //일반 몬스터 사망시 이벤트 함수
    public static Action Clear;    //게임클리어시 발생 이벤트
    private int diecount =0;   //일반 몬스터 사망 갯수 카운트
    AudioSource audio;          //사운드
    [SerializeField]
    AudioClip[] clip;           //오디오 클립
    public Image[] skillImage;         //스킬 이미지 
    Sound speaker;          //사운드
    private void Awake()
    {
        DieCount = () => { BossCondition(); };
        Clear = () => { GameClear(); };
        audio = GetComponent<AudioSource>();
        speaker = FindObjectOfType<Sound>();
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
        weaponShop.SetActive(true);
    }

    private void Update()
    {
        if(isBattle)
        {
            playTime += Time.deltaTime; //배틀 시작 순간부터 시간 계산.
        }
    }
    //게임오버
    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        isBattle = false;
        monsterGroup.SetActive(false); 
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        speaker.SoundByNum2(10);
    }
    //게임 클리어
    public void GameClear()
    {
        gamePanel.SetActive(false);
        clearPanel.SetActive(true);
        isBattle = false;
        player.isClear = true;
        player.anim.SetTrigger("doVictory");
        monsterGroup.SetActive(false);
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        speaker.SoundByNum2(14);
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
        {playerAmmoTxt.text = "" + player.ammo;}
        else if(player.equipWeapon.type == Weapon.Type.Melee)
        playerAmmoTxt.text = "" + player.ammo;
        else
            playerAmmoTxt.text = ""+ player.ammo;
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
            bossob.SetActive(true);
            cinema.SetActive(true);
            BossBGM();
        }
    }
    private void BossBGM() //보스 배경음악
    {
        //PlayOneShot을하면 딱 한번만 실행
        audio.clip = clip[0];
        audio.Play();
    }
}
