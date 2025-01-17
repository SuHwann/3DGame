using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    #region 변수
    public static Player instance;
    [SerializeField]
    private Transform cameraArm;        //플레이어를 따라다닐 카메라
    //Input Axis  값을 받을 전역변수 선언
    float hAxis;
    float vAxis;
    private bool wRun; // Run 변수
    private bool jDown; // jump 다운 변수
    private bool isJump; //무한 점프 방지용 변수
    private bool iDown;  //플레이어 무기 입수 변수
    private bool isSwap;  //무기 스왑 변수
    //플레이어가 무기를 교체 했는지 안했는지 체크 변수
    public bool[] weaponSwap;
    //플레이어가 공격을 받고 무적 타임을 위해 bool 변수 추가
    private bool isDamage;
    //플레이어의 무기관련 배열 함수 2개 선언
    public GameObject[] weapons;
    public bool[] hasWeapons;
    //공전하는 물체 배열 변수 생성 
/*    private GameObject[] grenades;*/
    //무기교체 
    bool sDown1;
    bool sDown2;
    bool sDown3;
    //벽충돌 플래그 bool 변수 생성
    bool isBorder;
    //Shoping 변수
    bool isShop;
    //공격 변수 (키입력 , 공격딜레이 , 공격준비)
    bool fDown;
    bool isFireReady = true;
    float fireDelay;
    //플레이어 죽음 bool 처리 변수 
    bool isDead;
    //플레이어 게임 클리어 판단
    public bool isClear;
    //NPC와 대화 중일경이 이동 통제 
    bool isTalk;
    //캐릭터 이동 속도
    [SerializeField]
    private float speed = 10f;
    //물리효과를 위해 Rigidbody 변수 선언 후 , 초기화
    Rigidbody rigid;
    //player Anim 변수
    public Animator anim;
    //트리거 된 아이템을 저장하기 위한 변수 선언
    GameObject nearObject;
    //현재 장비(Weapon)타입 변수 
    public Weapon equipWeapon;
    int equipWeaponIndex = -1;
    //플레이어의 MeshRenderer 배열 변수 추가 
    MeshRenderer[] meshs;
    //기존 플레이어가 기본적으로 들고있는 무기 
    [SerializeField]
    GameObject basicSword;
    //플레이어 피격 당했을때 Mesh 색 변화
    [SerializeField]
    SkinnedMeshRenderer skinnMesh;
    //player 능력치
    public static int ammo = 300;
    public static int coin = 9000;
    public static int health = 1000;
    public static int attack = 100;
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private int maxCoin;
    public int maxHealth;
    public GameManager manager; //GameManager 변수
    public static Action<int> SwordSwap; //무기 교체 이벤트 함수
    public static Action<bool> PlayerCol;
    public static Action<int> GiveJewel; //플레이어 보석 얻기
    public bool[] jewel; //보석 아이템을 가지고있는지 판단
    Sound speaker; //스피커
    public bool isSkill; //스킬 사용중일때 데미지 방지
    #endregion
    #region 함수
    private void Awake()
    {
        instance = this; 
        rigid = GetComponent<Rigidbody>();//자기 자신의 rigid를 가져온다
        anim = GetComponentInChildren<Animator>(); //Animaltor 변수를 GetCommponentChildren()으로 초기화
        meshs = GetComponentsInChildren<MeshRenderer>(); //player 메쉬 초기화 
        equipWeapon = basicSword.GetComponent<Weapon>(); //처음엔 기본칼을 사용한다.
        StartCoroutine(MoveUpdate());
        Weapon.Damage(attack);//Weapon 스크립트에 플레이어 공격력 전달.
        SwordSwap = (int i) => { SwordChange(i); }; //람다식 무기 교체 이벤트 함수
        GiveJewel = (int i) => { JewelGive(i); }; //람다식 보석 추가 함수
        speaker = FindObjectOfType<Sound>();
    }
    IEnumerator MoveUpdate()
    {
        while(true)
        {
            Attack();
            GetInput();
            Move();
            Jump();
            Interation();
            Swap();
            yield return null;  
        }
    }
    //플레이어 입력기능
    protected void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); 
        vAxis = Input.GetAxisRaw("Vertical");
        wRun = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        fDown = Input.GetButton("Fire1");             
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    //Player 이동 기능 
    protected void Move()
    {
        //플레이어 vec
        Vector2 moveInput = new Vector2(hAxis, vAxis);
        //카메라 vec
        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 lookRIght = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        //최종 vec
        Vector3 moveDir = lookForward * moveInput.y + lookRIght * moveInput.x;
        bool isMove = moveInput.magnitude != 0;
        anim.SetBool("isRun", isMove);
        anim.SetBool("isWalk", wRun); //walk 다운
        //플레이어 정지 상황
        if (isSwap || !isFireReady || fDown || isDead || isSkill || isClear)
        {
            moveDir = Vector3.zero;
        }
        //플레이어 대쉬
        if (!isDead && moveDir != Vector3.zero)
        {
            transform.forward = moveDir;
            if (!isBorder) { transform.position += moveDir * speed * (wRun ? 1.5f : 1f) * Time.deltaTime; };
        }
    }
    protected void Jump() //player 점프 기능
    {
        if(jDown && !isJump && !isSwap && !isShop && !isDead && !isTalk &&!isClear)
        {
            rigid.AddForce(Vector3.up * 15 , ForceMode.Impulse); // 즉발적인 Impulse로 한다.
            anim.SetBool("isJump", true); // 점프가 발동되면 isjump true
            anim.SetTrigger("doJump");
            isJump = true;
            speaker.SoundByNum2(6);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //바닦이 닿으면 Jump를 다시 할수 있다 
       if(collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    //other가 Weapon 이거나 Shop 이면 nearObject에 저장,other가 Item 이면 item에 저장하는 기능
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Shop") || other.CompareTag("NPC") || other.CompareTag("WeaponShop"))
        {
            nearObject = other.gameObject;
        }
        if (iDown) //E키일때만 아이템 드랍 가능
        {
            switch (other.tag)
            {
                case "Item":
                    {
                        Item item = other.GetComponent<Item>();
                        switch (item.type)
                        {
                            //아이템 능력치 플레이어에게 적용
                            case Item.Type.Ammo:
                                ammo += item.value;
                                if (ammo > maxAmmo)
                                    ammo = maxAmmo;
                                break;
                            case Item.Type.Coin:
                                coin += item.value;
                                if (coin > maxCoin)
                                    coin = maxCoin;
                                print("현재 코인은: " + coin);
                                break;
                            case Item.Type.Heart:
                                health += item.value;
                                if (health > maxHealth)
                                    health = maxHealth;
                                break;
                            case Item.Type.Attack:
                                attack += item.value;
                                break;
                        }
                        speaker.SoundByNum2(9);
                        //먹은 아이템은 삭제
                        Destroy(other.gameObject);
                        break;
                    }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSkill)
        {
            switch (other.tag)
            {
                //Attack 스크립트 재활용하여 데미지 적용 
                case "EnemyFar":
                    {
                        if (!isDamage)
                        {
                            Attack enemySlash = other.GetComponent<Attack>();
                            health -= enemySlash.damage;
                            bool isBoosAtk = other.name == "EnemyFar";
                            StartCoroutine(OnDamage(isBoosAtk));
                        }
                        break;
                    }
            }
        }
    }
    //플레이어가 공격 받았을때 
    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        speaker.SoundByNum(2);
        foreach (MeshRenderer mesh in meshs) //공격을 받았을때 색변화 
        {
            skinnMesh.material.color = Color.red;
            mesh.material.color = Color.red;
        }
        if (isBossAtk)
            rigid.AddForce(transform.forward * -25, ForceMode.Impulse);
        if (health <= 0 && !isDead) //플레이어 체력이 0 이하면 사망 처리 
        {
            OnDie();
        }
        if (!isDead) { anim.SetTrigger("doDamage"); }
        yield return new WaitForSeconds(1f);// 무적 시간 
        isDamage = false;
        foreach (MeshRenderer mesh in meshs) //다시 돌아옴
        {
            skinnMesh.material.color = Color.white;
            mesh.material.color = Color.white;
        }
        if (isBossAtk)
            rigid.velocity = Vector3.zero;
    }
    void OnDie()
    {
        speaker.SoundByNum(4);
        anim.SetTrigger("doDie");
        isDead = true;
        manager.GameOver();
    }
    //other 영역에서 벗어났을땐 null을 저장한다.
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Shop"))
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            nearObject = null;
            isShop = false;
        }
        else if(other.CompareTag("NPC"))
        {
            NPCTalk talk = nearObject.GetComponent<NPCTalk>();
            talk.Exit();
            nearObject= null;
            isTalk= false;
        }
        else if(other.CompareTag("WeaponShop"))
        {
            WeaponShop WeaponShop = nearObject.GetComponent<WeaponShop>();
            WeaponShop.Exit();
            nearObject = null;
            isShop = false;
        }
    }
    //무기교체
    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;
        int weaponIndex = -1;
        if(sDown1) weaponIndex = 0;
        if(sDown2) weaponIndex = 1;
        if(sDown3) weaponIndex = 2;
        //셋중 하나만 눌러도 작동 or 조건 작성
        if((sDown1 || sDown2 || sDown3) && !isJump && !wRun && !isTalk)
        {
            basicSword.SetActive(false);//기존의 칼도 비활성화 시킨다
            equipWeaponIndex = weaponIndex;
            if (equipWeapon != null) //equipWeapon이 null이 아니면 기존의 장비는 비활성화
            {
              equipWeapon.gameObject.SetActive(false);
            }
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            anim.SetTrigger("doSwap");
            isSwap = true;
            weaponSwap[weaponIndex] = true;
            manager.skillImage[weaponIndex].gameObject.SetActive(true);
            Invoke("SwapOut", 0.4f);
        }
    }
    //shop,npc,weapon UI띄우기
    void Interation()
    {
        //E(iDown)를 활성화 되고 nearObject 가 null이 아니고 점프와 Run이 아닐때 
        if(iDown && nearObject != null && !isJump && !wRun )
        {
            if (nearObject.CompareTag("Shop"))
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                isShop = true;
            }
            else if (nearObject.CompareTag("NPC"))
            {
                NPCTalk npc = nearObject.GetComponent<NPCTalk>();
                npc.Enter(this);
                isTalk = true;
            }
            else if(nearObject.CompareTag("WeaponShop"))
            {
                WeaponShop weaponShop = nearObject.GetComponent<WeaponShop>();
                weaponShop.Enter(this);
                isShop = true;
            }
        }
    }
    //상점의 통한 무기교체
    public void SwordChange(int i)
    {
        hasWeapons[i] = true;
    }
    //Swap에 Invoke로 사용 
    void SwapOut()
    {
        isSwap = false;
    }
    //플레이어 자동 회전 방지 
    private void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero; //angularVelocity 는 물리 회전 속도 
    }
    private void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }
    //공격 함수 (무기가 있을때만 실행)
    void Attack()
    {
        if (equipWeapon == null)
            return;
        //공격시간을 더해주고 공격가능 여부 확인
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isSwap && !isShop &&!isDead && !isTalk && !isClear)
        {
            equipWeapon.Use();
            anim.SetTrigger("doSwing" ) ;
            fireDelay = 0;
            speaker.SoundByNum2(5);
        }
    }
    //벽 통과 방지
    void StopToWall()
    {
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    //플레이어 보석 증가 
    public void JewelGive(int i)
    {
        switch (i)
        { 
            case 0 : //GreenSword Get
                jewel[0] = true;
                break;
            case 1 : //RedSword Get
                jewel[1] = true;
                break;
            case 2: //BlueSword Get
                jewel[2] = true;
                break;
        }
    }
    #endregion
}
