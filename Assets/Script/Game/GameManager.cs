using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuCam, gameCam; //���� ī�޶�
    [SerializeField]
    Player player; //���� �÷��̾�
    [SerializeField]
    GameObject bossob; //���� 
    [SerializeField]
    GameObject cinema; //�ó׸���
    [SerializeField]
    int stage;       //���� �������� ����
    [SerializeField]
    float playTime;  //���� �÷��� �ð�
    [SerializeField]
    bool isBattle;      //���� ��Ȳ���� �ƴ��� �Ǵ�
    [SerializeField]
    GameObject monsterGroup; //���� �׷�
    [SerializeField]
    GameObject[] monsters; //�Ϲ� ���� �迭
    public GameObject menuPanel,gamePanel,overPanel;
    public Text stageTxt,playTimeTxt,playerHealthTxt,playerAmmoTxt,playerCoinTxt;
    public RectTransform bossHealthGroup,bossHealthBar;
    [SerializeField]
    GameObject itemShop;     //������ ��
    [SerializeField]
    GameObject weaponShop;   //���� ��
    public static Action DieCount; //�Ϲ� ���� ����� �̺�Ʈ �Լ�
    private int diecount =0;   //�Ϲ� ���� ��� ���� ī��Ʈ
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
            playTime += Time.deltaTime; //��Ʋ ���� �������� �ð� ���.
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
        //���� �������� UI
        stageTxt.text = "STAGE" + stage;
        //�ð� ���
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime  % 60);
        //Player �÷��� �ð�
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
        //������ ������ ���� UI����
        if(bossob.activeSelf)
        {
            bossHealthGroup.anchoredPosition = Vector3.down * 30f;
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 200f;
        }
    }
    void BossCondition() //������ ������ ��Ȳ
    {
        diecount++;
        if(monsters.Length == diecount)
        {
            print("�Ϲ� ���� ���� ��� ���� ����");
            bossob.SetActive(true);
            cinema.SetActive(true); 
        }
    }
}
