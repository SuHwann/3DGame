using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
    //���� ���� (Ű�Է� , ���ݵ����� , �����غ�)
    bool fDown;
    bool isFireReady = true;
    float fireDelay;
    //ĳ���� �̵� �ӵ�
    [SerializeField]
    private float speed = 10f;
    //move ����
    Vector3 moveVec;

    //����ȿ���� ���� Rigidbody ���� ���� �� , �ʱ�ȭ
    Rigidbody rigid;
    //player Anim ����
    Animator anim;
    //Ʈ���� �� �������� �����ϱ� ���� ���� ����
    GameObject nearObject;
    //���� ���(Weapon)Ÿ�� ���� 
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    //�÷��̾��� MeshRenderer �迭 ���� �߰� 
    MeshRenderer[] meshs;
    SkinnedMeshRenderer[] skinnmeshs;
    //���� �÷��̾ �⺻������ ����ִ� ���� 
    [SerializeField]
    GameObject basicSword;
    //player �ɷ�ġ
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
        rigid = GetComponent<Rigidbody>();//�ڱ� �ڽ��� rigid�� �����´�
        anim = GetComponentInChildren<Animator>(); //Animaltor ������ GetCommponentChildren()���� �ʱ�ȭ
        meshs = GetComponentsInChildren<MeshRenderer>(); //player �޽� �ʱ�ȭ 
        skinnmeshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        equipWeapon = basicSword.GetComponent<Weapon>(); //ó���� �⺻Į�� ����Ѵ�.
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
    protected void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//normalized �� ���Ⱚ 1�� ����

        if(isSwap || !isFireReady)
        {
            moveVec = Vector3.zero;
        }
        transform.position += moveVec * speed * (wRun ? 1.5f : 1f) * Time.deltaTime; //Time.deltaTime ���� �̵��ӵ� ����

        anim.SetBool("isRun", moveVec != Vector3.zero);//SetBool() �Լ��� �Ķ���� ���� ����
        anim.SetBool("isWalk", wRun); //walk �ٿ�

    }
    //Player�� �̵� ���⿡ ���� �ٷ� ���� LookAt
    protected void Trun()
    {
        transform.LookAt(transform.position + moveVec);
    }
    protected void Jump()
    {
        if(jDown && !isJump && !isSwap)
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
       if(collision.gameObject.name == "Terrain")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    //other�� Weapon �̸� nearObject�� ����
    private void OnTriggerStay(Collider other)
    {
        if(other.tag =="Weapon")
        {
            nearObject = other.gameObject;
        }
    }
    //other�� Item �̸� item�� �����ϴ� ���
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Item")
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
                    if(coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                    //��ź�� �Ծ�����
/*                case Item.Type.Grenade:
                    hasGrenades += item.value;
                    if(hasGrenades > maxHasGrenades)
                        hasGrenades = maxHasGrenades;
                    break;*/
            }
            //���� �������� ����
            Destroy(other.gameObject);
        }
        //Slash ��ũ��Ʈ ��Ȱ���Ͽ� ������ ���� 
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
    //other �������� ������� null�� �����Ѵ�.
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
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
        if((sDown1 || sDown2 || sDown3) && !isJump && !wRun)
        {
            basicSword.SetActive(false);//������ Į�� ��Ȱ��ȭ ��Ų��
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

    //���� �Լ�
    void Interation()
    {
        //E(iDown)�� Ȱ��ȭ �ǰ� nearObject �� null�� �ƴϰ� ������ Run�� �ƴҶ� 
        if(iDown && nearObject != null && !isJump && !wRun)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                Destroy(nearObject);//������Ʈ ����
            }
        }
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
    }
    //���� �Լ� (���Ⱑ �������� ����)
    void Attack()
    {
        if (equipWeapon == null)
            return;
        //���ݽð��� �����ְ� ���ݰ��� ���� Ȯ��
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
