using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject menuPanel, gamePanel;
    public Text maxScoreTxt,scoreTxt,stageTxt,playTimeTxt,playerHealthTxt,playerAmmoTxt,playerCoinTxt;
    public Image weapon1Image, weapon2Image, weapon3Image, weapon4Image;
    public Text enemyAtxt, enemyBtxt, enemyCtxt;
    public RectTransform bossHealthGroup,bossHealthBar;

    private void Awake()
    {
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }
    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }
    private void LateUpdate()
    {
        scoreTxt.text = string.Format("{0:n0)", player.score);
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}",player.coin);
        if(player.equipWeapon == null)
        {playerAmmoTxt.text = "- / " + player.ammo;}
        else if(player.equipWeapon.type == Weapon.Type.Melee)
        playerAmmoTxt.text = "- /" + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " /" + player.ammo;

        weapon1Image.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Image.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Image.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weapon4Image.color = new Color(1, 1, 1, player.hasWeapons[3] ? 1 : 0);
    }
}
