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
    public GameObject menuPanel,gamePanel,overPanel,clearPanel;
    public Text stageTxt,playTimeTxt,playerHealthTxt,playerAmmoTxt,playerCoinTxt, bossText;
    public RectTransform bossHealthGroup,bossHealthBar;
    [SerializeField]
    GameObject itemShop;     //������ ��
    [SerializeField]
    GameObject weaponShop;   //���� ��
    public static Action DieCount; //�Ϲ� ���� ����� �̺�Ʈ �Լ�
    public static Action Clear;    //����Ŭ����� �߻� �̺�Ʈ
    private int diecount =0;   //�Ϲ� ���� ��� ���� ī��Ʈ
    AudioSource audio;          //����
    [SerializeField]
    AudioClip[] clip;           //����� Ŭ��
    public Image[] skillImage;         //��ų �̹��� 
    Sound speaker;          //����
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
            playTime += Time.deltaTime; //��Ʋ ���� �������� �ð� ���.
        }
    }
    //���ӿ���
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
    //���� Ŭ����
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
        {playerAmmoTxt.text = "" + player.ammo;}
        else if(player.equipWeapon.type == Weapon.Type.Melee)
        playerAmmoTxt.text = "" + player.ammo;
        else
            playerAmmoTxt.text = ""+ player.ammo;
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
            bossob.SetActive(true);
            cinema.SetActive(true);
            BossBGM();
        }
    }
    private void BossBGM() //���� �������
    {
        //PlayOneShot���ϸ� �� �ѹ��� ����
        audio.clip = clip[0];
        audio.Play();
    }
}
