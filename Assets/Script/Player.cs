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
    //�÷��̾��� ������� �迭 �Լ� 2�� ����

    public GameObject[] weapons;
    public bool[] hasWeapons;
    //���ⱳü 
    bool sDown1;
    bool sDown2;
    bool sDown3;
    //ĳ���� �̵� �ӵ�
    private float speed = 10f;
    //move ����
    Vector3 moveVec; 

    //����ȿ���� ���� Rigidbody ���� ���� �� , �ʱ�ȭ
    Rigidbody rigid;
    //Player Anim ����
    Animator anim;
    //Ʈ���� �� �������� �����ϱ� ���� ���� ����
    GameObject nearObject;
    //������ ������ ���⸦ �����ϴ� ������ �����ϰ� Ȱ���ϱ�
    GameObject equipWeapon;
    int equipWeaponIndex = -1;
    //���� �÷��̾ �⺻������ ����ִ� ���� 
    [SerializeField]
    GameObject basicSword;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();//�ڱ� �ڽ��� rigid�� �����´�
        anim = GetComponentInChildren<Animator>(); //Animaltor ������ GetCommponentChildren()���� �ʱ�ȭ
    }

    private void Update()
    {
        GetInput();
        Move();
        Trun();
        Jump();
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
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    protected void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//normalized �� ���Ⱚ 1�� ����

        if(isSwap)
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
        if(jDown && moveVec == Vector3.zero && !isJump && !isSwap)
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
    //other �� Weapon �̸� nearObject�� ����
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Weapon")
        {
            nearObject = other.gameObject;
        }
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
              equipWeapon.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);

            anim.SetTrigger("doSwap");
            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    //���� �Լ�
    void Interation()
    {
        //E(iDown)�� Ȱ��ȭ �ǰ� nearObject �� null�� �ƴϰ� ������ Run�� �ƴҶ� 
        if(iDown && nearObject != null && !isJump & !wRun)
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
}
