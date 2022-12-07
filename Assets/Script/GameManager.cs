using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuCam, gameCam;
    [SerializeField]
    Player player;
    [SerializeField]
    Boss boss;
    [SerializeField]
    int stage;
    [SerializeField]
    float playTime;
    [SerializeField]
    bool isBattle;      //지금 전투중인가
    [SerializeField]
    int enemyCntA, enemyCntB, enemyCntC;
    public GameObject menuPanel, gamePanel , overPanel;
    public Text maxScoreTxt,scoreTxt,stageTxt,playTimeTxt,playerHealthTxt,playerAmmoTxt,playerCoinTxt;
    public Image weapon1Image, weapon2Image, weapon3Image;
    public Text enemyAtxt, enemyBtxt, enemyCtxt;
    public RectTransform bossHealthGroup,bossHealthBar;
    public GameObject itemShop;     //아이템 상점
    //public GameObject itemweaponShop; //무기 상점
    public GameObject startZone;    //스타트 존
    public Text curScoreText; //최종 점수
    public Text bestText;  //최고점수
    private void Awake()
    {
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
        if(PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MasScore", 0);
        }
    }
    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }
    private void Update()
    {
        if(isBattle)
        {
            playTime += Time.deltaTime; //delta를 꾸준히 더해주면 그게 곧 시간이 된다.
        }
    }
    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if(player.score > maxScore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.score);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    private void LateUpdate()
    {
        //상단UI
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE" + stage;
        //시간
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime  % 60);
        //Play시간 
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
        //스킬 UI
        weapon1Image.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        //-나중에 UI가 정해지면 수정하도록-
/*        weapon2Image.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Image.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);*/
        //몬스터 숫자 UI
        enemyAtxt.text = enemyCntA.ToString();
        enemyBtxt.text = enemyCntB.ToString();
        enemyCtxt.text = enemyCntC.ToString();
        //보스 몬스터 체력
        if(boss != null)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30f;
            bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200f;
        }
    }
}
