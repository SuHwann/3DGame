using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Input Axis  값을 받을 전역변수 선언
    float hAxis;
    float vAxis;
    private bool wRun; // Run 변수
    private bool jDown; // jump 다운 변수
    private bool isJump; //무한 점프 방지용 변수
    private bool iDown;  //플레이어 무기 입수 변수
    private bool isSwap;  //무기 스왑 변수
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
    //공격 변수 (키입력 , 공격딜레이 , 공격준비)
    bool fDown;
    bool isFireReady = true;
    float fireDelay;
    //캐릭터 이동 속도
    [SerializeField]
    private float speed = 10f;
    //move 벡터
    Vector3 moveVec;

    //물리효과를 위해 Rigidbody 변수 선언 후 , 초기화
    Rigidbody rigid;
    //player Anim 변수
    Animator anim;
    //트리거 된 아이템을 저장하기 위한 변수 선언
    GameObject nearObject;
    //현재 장비(Weapon)타입 변수 
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    //플레이어의 MeshRenderer 배열 변수 추가 
    MeshRenderer[] meshs;
    SkinnedMeshRenderer[] skinnmeshs;
    //기존 플레이어가 기본적으로 들고있는 무기 
    [SerializeField]
    GameObject basicSword;
    //player 능력치
    [SerializeField]
    private int ammo;
    [SerializeField]
    private int coin;
    [SerializeField]
    private int health;
    [SerializeField]
    private int hasGrenades;
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private int maxCoin;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int maxHasGrenades;
 
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();//자기 자신의 rigid를 가져온다
        anim = GetComponentInChildren<Animator>(); //Animaltor 변수를 GetCommponentChildren()으로 초기화
        meshs = GetComponentsInChildren<MeshRenderer>(); //player 메쉬 초기화 
        skinnmeshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        equipWeapon = basicSword.GetComponent<Weapon>(); //처음엔 기본칼을 사용한다.
    }

    private void Update()
    {
        GetInput();
        Move();
        Trun();
        Jump();
        Attack();
        Interation();
        Swap();
    }
    protected void GetInput()
    {
        //Axis 값을 정수로 반환하는 함수
        hAxis = Input.GetAxisRaw("Horizontal"); 
        vAxis = Input.GetAxisRaw("Vertical");
        wRun = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        fDown = Input.GetButton("Fire1");                //꾹 눌러도 자동으로 나감 
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    protected void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//normalized 로 방향값 1로 보정

        if(isSwap || !isFireReady)
        {
            moveVec = Vector3.zero;
        }
        transform.position += moveVec * speed * (wRun ? 1.5f : 1f) * Time.deltaTime; //Time.deltaTime 으로 이동속도 조절

        anim.SetBool("isRun", moveVec != Vector3.zero);//SetBool() 함수로 파라메터 값을 설정
        anim.SetBool("isWalk", wRun); //walk 다운

    }
    //Player가 이동 방향에 따라 바로 본다 LookAt
    protected void Trun()
    {
        transform.LookAt(transform.position + moveVec);
    }
    protected void Jump()
    {
        if(jDown && !isJump && !isSwap)
        {
            rigid.AddForce(Vector3.up * 15 , ForceMode.Impulse); // 즉발적인 Impulse로 한다.
            anim.SetBool("isJump", true); // 점프가 발동되면 isjump true
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //바닦이 닿으면 Jump를 다시 할수 있다 
       if(collision.gameObject.name == "Terrain")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    //other가 Weapon 이면 nearObject에 저장
    private void OnTriggerStay(Collider other)
    {
        if(other.tag =="Weapon")
        {
            nearObject = other.gameObject;
        }
    }
    //other가 Item 이면 item에 저장하는 기능
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Item")
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
                    if(coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                    //폭탄을 먹었을때
/*                case Item.Type.Grenade:
                    hasGrenades += item.value;
                    if(hasGrenades > maxHasGrenades)
                        hasGrenades = maxHasGrenades;
                    break;*/
            }
            //먹은 아이템은 삭제
            Destroy(other.gameObject);
        }
        //Slash 스크립트 재활용하여 데미지 적용 
        else if (other.tag == "EnemyFar")
        {
            if(!isDamage)
            {
                Slash enemySlash = other.GetComponent<Slash>();
                health -= enemySlash.damage;
                bool isBoosAtk = other.name == "BossMeleeArea";
                StartCoroutine(OnDamage(isBoosAtk));
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
    //몇개는 MeshRenderer이고 몇개는 SkinMeshRenderer라 이렇게 그냥 따로 선언.
    [SerializeField]
    SkinnedMeshRenderer skinnMesh;
    //플레이어가 공격 받았을때 
    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        foreach (MeshRenderer mesh in meshs) //공격을 받았을때 색변화 
        {
            skinnMesh.material.color = Color.red;
            mesh.material.color = Color.red;
        }
        if (isBossAtk)
            rigid.AddForce(transform.forward * -25, ForceMode.Impulse);
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
    //other 영역에서 벗어났을땐 null을 저장한다.
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
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
        if((sDown1 || sDown2 || sDown3) && !isJump && !wRun)
        {
            basicSword.SetActive(false);//기존의 칼도 비활성화 시킨다
            if (equipWeapon != null)
            {
              equipWeapon.gameObject.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");
            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    //무기 입수
    void Interation()
    {
        //E(iDown)를 활성화 되고 nearObject 가 null이 아니고 점프와 Run이 아닐때 
        if(iDown && nearObject != null && !isJump && !wRun)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                Destroy(nearObject);//오브젝트 삭제
            }
        }
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
    }
    //공격 함수 (무기가 있을때만 실행)
    void Attack()
    {
        if (equipWeapon == null)
            return;
        //공격시간을 더해주고 공격가능 여부 확인
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doSlash");
            fireDelay = 0;
        }
    }
}
