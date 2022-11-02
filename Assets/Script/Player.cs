using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Input Axis  ���� ���� �������� ����
    float hAxis;
    float vAxis;
    private bool wDown; // walk �ٿ� ����
    private bool jDown; // jump �ٿ� ����
    private bool isJump; //���� ���� ������ ����
    //ĳ���� �̵� �ӵ�
    private float speed = 10f;
    //move ����
    Vector3 moveVec; 

    //����ȿ���� ���� Rigidbody ���� ���� �� , �ʱ�ȭ
    Rigidbody rigid;
    //Player Anim ����
    Animator anim;
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
    }
    protected void GetInput()
    {
        //Axis ���� ������ ��ȯ�ϴ� �Լ�
        hAxis = Input.GetAxisRaw("Horizontal"); 
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }
    protected void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//normalized �� ���Ⱚ 1�� ����

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; //Time.deltaTime ���� �̵��ӵ� ����

        anim.SetBool("isRun", moveVec != Vector3.zero);//SetBool() �Լ��� �Ķ���� ���� ����
        anim.SetBool("isWalk", wDown); //walk �ٿ�
    }
    //Player�� �̵� ���⿡ ���� �ٷ� ���� LookAt
    protected void Trun()
    {
        transform.LookAt(transform.position + moveVec);
    }
    protected void Jump()
    {
        if(jDown && ! isJump)
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
}
