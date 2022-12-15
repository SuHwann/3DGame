using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform cameraArm;        //�÷��̾ ����ٴ� ī�޶�
    //Input Axis  ���� ���� �������� ����
    float hAxis;
    float vAxis;
    private bool wRun; // Run ����
    private bool jDown; // jump �ٿ� ����
    private bool isJump; //���� ���� ������ ����
    private bool iDown;  //�÷��̾� ���� �Լ� ����
    private bool isSwap;  //���� ���� ����
    //�÷��̾ ������ �ް� ���� Ÿ���� ���� bool ���� �߰�
    private bool isDamage;
    //�÷��̾��� ������� �迭 �Լ� 2�� ����
    public GameObject[] weapons;
    public bool[] hasWeapons;
    //�����ϴ� ��ü �迭 ���� ���� 
/*    private GameObject[] grenades;*/
    //���ⱳü 
    bool sDown1;
    bool sDown2;
    bool sDown3;
    //���浹 �÷��� bool ���� ����
    bool isBorder;
    //Shoping ����
    bool isShop;
    //���� ���� (Ű�Է� , ���ݵ����� , �����غ�)
    bool fDown;
    bool isFireReady = true;
    float fireDelay;
    //�÷��̾� ���� bool ó�� ���� 
    bool isDead;
    //NPC�� ��ȭ ���ϰ��� �̵� ���� 
    bool isTalk;
    //ĳ���� �̵� �ӵ�
    [SerializeField]
    private float speed = 10f;
    //����ȿ���� ���� Rigidbody ���� ���� �� , �ʱ�ȭ
    Rigidbody rigid;
    //player Anim ����
    Animator anim;
    //Ʈ���� �� �������� �����ϱ� ���� ���� ����
    GameObject nearObject;
    //���� ���(Weapon)Ÿ�� ���� 
    public Weapon equipWeapon;
    int equipWeaponIndex = -1;
    //�÷��̾��� MeshRenderer �迭 ���� �߰� 
    MeshRenderer[] meshs;
    SkinnedMeshRenderer[] skinnmeshs; //�÷��̾� �޽�
    //���� �÷��̾ �⺻������ ����ִ� ���� 
    [SerializeField]
    GameObject basicSword;
    //player �ɷ�ġ
    public int ammo;
    public int coin;
    public int health;
    public int attack;
    [SerializeField]
    private int hasGrenades;        //����ź ���� ����
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private int maxCoin;
    public int maxHealth;
    [SerializeField]
    private int maxHasGrenades;
    public GameManager manager; //GameManager ����
    public static Action<int> SwordSwap; //���� ��ü �̺�Ʈ �Լ�
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();//�ڱ� �ڽ��� rigid�� �����´�
        anim = GetComponentInChildren<Animator>(); //Animaltor ������ GetCommponentChildren()���� �ʱ�ȭ
        meshs = GetComponentsInChildren<MeshRenderer>(); //player �޽� �ʱ�ȭ 
        skinnmeshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        equipWeapon = basicSword.GetComponent<Weapon>(); //ó���� �⺻Į�� ����Ѵ�.
        StartCoroutine(MoveUpdate());
        Weapon.Damage(attack);//Weapon ��ũ��Ʈ�� �÷��̾� ���ݷ� ����.
        SwordSwap = (int i) => { SwordChange(i); }; //���ٽ� ���� ��ü �̺�Ʈ �Լ�
    }
    IEnumerator MoveUpdate()
    {
        while(true)
        {
            Attack();
            GetInput();
            Move();
            LookAround();
            Jump();
            Interation();
            Swap();
            yield return null;  
        }
    }
    //�÷��̾� �Է±��
    protected void GetInput()
    {
        //Axis ���� ������ ��ȯ�ϴ� �Լ�
        hAxis = Input.GetAxisRaw("Horizontal"); 
        vAxis = Input.GetAxisRaw("Vertical");
        wRun = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        fDown = Input.GetButton("Fire1");                //�� ������ �ڵ����� ���� 
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    //Player �̵� ��� 
    protected void Move()
    {   
        Vector2 moveInput = new Vector2(hAxis, vAxis);
        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 lookRIght = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRIght * moveInput.x;
        bool isMove = moveInput.magnitude != 0;
        anim.SetBool("isRun", isMove);
        anim.SetBool("isWalk", wRun); //walk �ٿ�

        if (isSwap || !isFireReady || fDown || isDead)
        {
            moveDir = Vector3.zero;
        }
        if (!isDead && moveDir != Vector3.zero)
        {
            transform.forward = moveDir;
            if (!isBorder){transform.position += moveDir * speed * (wRun ? 1.5f : 1f) * Time.deltaTime;};//Time.deltaTime ���� �̵��ӵ� ����
        }
    }
    protected void Jump()
    {
        if(jDown && !isJump && !isSwap && !isShop && !isDead && !isTalk)
        {
            rigid.AddForce(Vector3.up * 15 , ForceMode.Impulse); // ������� Impulse�� �Ѵ�.
            anim.SetBool("isJump", true); // ������ �ߵ��Ǹ� isjump true
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //�ٴ��� ������ Jump�� �ٽ� �Ҽ� �ִ� 
       if(collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    //other�� Weapon �̰ų� Shop �̸� nearObject�� ����,other�� Item �̸� item�� �����ϴ� ���
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Weapon") || other.CompareTag("Shop") || other.CompareTag("NPC"))
        {
            nearObject = other.gameObject;
        }
        if (iDown) //EŰ�϶��� ������ ��� ����
        {
            switch (other.tag)
            {
                case "Item":
                    {
                        Item item = other.GetComponent<Item>();
                        switch (item.type)
                        {
                            //������ �ɷ�ġ �÷��̾�� ����
                            case Item.Type.Ammo:
                                ammo += item.value;
                                if (ammo > maxAmmo)
                                    ammo = maxAmmo;
                                break;
                            case Item.Type.Coin:
                                coin += item.value;
                                if (coin > maxCoin)
                                    coin = maxCoin;
                                print("���� ������: " + coin);
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
                        //���� �������� ����
                        Destroy(other.gameObject);
                        break;
                    }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            //Slash ��ũ��Ʈ ��Ȱ���Ͽ� ������ ���� 
            case "EnemyFar":
            {
                if(!isDamage)
                {
                    Slash enemySlash = other.GetComponent<Slash>();
                    health -= enemySlash.damage;
                    bool isBoosAtk = other.name == "EnemyFar";
                    StartCoroutine(OnDamage(isBoosAtk));
                }
                break;
            }
        }
    }
    //��� MeshRenderer�̰� ��� SkinMeshRenderer�� �̷��� �׳� ���� ����.
    [SerializeField]
    SkinnedMeshRenderer skinnMesh;
    //�÷��̾ ���� �޾����� 
    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        foreach (MeshRenderer mesh in meshs) //������ �޾����� ����ȭ 
        {
            skinnMesh.material.color = Color.red;
            mesh.material.color = Color.red;
        }
        if (isBossAtk)
            rigid.AddForce(transform.forward * -25, ForceMode.Impulse);
        if (health <= 0 && !isDead) //�÷��̾� ü���� 0 ���ϸ� ��� ó�� 
        {
            OnDie();
        }
        yield return new WaitForSeconds(1f);// ���� �ð� 
        isDamage = false;
        foreach (MeshRenderer mesh in meshs) //�ٽ� ���ƿ�
        {
            skinnMesh.material.color = Color.white;
            mesh.material.color = Color.white;
        }
        if (isBossAtk)
            rigid.velocity = Vector3.zero;
    }
    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        manager.GameOver();
    }
    //other �������� ������� null�� �����Ѵ�.
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            nearObject = null;
        }
        else if(other.CompareTag("Shop"))
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
    }
    //���ⱳü
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
        //���� �ϳ��� ������ �۵� or ���� �ۼ�
        if((sDown1 || sDown2 || sDown3) && !isJump && !wRun && !isTalk)
        {
            basicSword.SetActive(false);//������ Į�� ��Ȱ��ȭ ��Ų��
            if (equipWeapon != null) //equipWeapon�� null�� �ƴϸ� ������ ���� ��Ȱ��ȭ
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
    //���� �Լ�
    void Interation()
    {
        //E(iDown)�� Ȱ��ȭ �ǰ� nearObject �� null�� �ƴϰ� ������ Run�� �ƴҶ� 
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
        }
    }
    //������ ���� ���ⱳü
    public void SwordChange(int i)
    {
        hasWeapons[i] = true;
    }
    //Swap�� Invoke�� ��� 
    void SwapOut()
    {
        isSwap = false;
    }
    //�÷��̾� �ڵ� ȸ�� ���� 
    private void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero; //angularVelocity �� ���� ȸ�� �ӵ� 
    }
    private void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }
    //���� �Լ� (���Ⱑ �������� ����)
    void Attack()
    {
        if (equipWeapon == null)
            return;
        //���ݽð��� �����ְ� ���ݰ��� ���� Ȯ��
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isSwap && !isShop &&!isDead && !isTalk)
        {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doSlash");
            fireDelay = 0;
        }
    }
    //Cameraȸ�� ���
    private void LookAround()
    {
        if(Input.GetMouseButton(1)) //��Ŭ���ø�
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //���콺 ���Ʒ� ��ġ�� vector2�� mouseDelta�� �����Ѵ�.
            Vector3 camAngle = cameraArm.rotation.eulerAngles; //ī�޶� position�� ���� Euler������ ��ȯ�� �д�.
            float x = camAngle.x - mouseDelta.y;
            //���� ����
            if (x < 180f)
            {
                x = Mathf.Clamp(x, -1f, 70f);
            }
            else
            {
                x = Mathf.Clamp(x, 355f, 361f);
            }
            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);//camAngle�� ���ο�� cameraArm.rotation�� �־��ش�.
        }
    }
    //�� ��� ����
    void StopToWall()
    {
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

}
