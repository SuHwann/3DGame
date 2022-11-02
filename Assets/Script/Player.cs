using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Input Axis  값을 받을 전역변수 선언
    float hAxis;
    float vAxis;
    private bool wDown; // walk 다운 변수
    private bool jDown; // jump 다운 변수
    private bool isJump; //무한 점프 방지용 변수
    //캐릭터 이동 속도
    private float speed = 10f;
    //move 벡터
    Vector3 moveVec; 

    //물리효과를 위해 Rigidbody 변수 선언 후 , 초기화
    Rigidbody rigid;
    //Player Anim 변수
    Animator anim;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();//자기 자신의 rigid를 가져온다
        anim = GetComponentInChildren<Animator>(); //Animaltor 변수를 GetCommponentChildren()으로 초기화
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
        //Axis 값을 정수로 반환하는 함수
        hAxis = Input.GetAxisRaw("Horizontal"); 
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }
    protected void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//normalized 로 방향값 1로 보정

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime; //Time.deltaTime 으로 이동속도 조절

        anim.SetBool("isRun", moveVec != Vector3.zero);//SetBool() 함수로 파라메터 값을 설정
        anim.SetBool("isWalk", wDown); //walk 다운
    }
    //Player가 이동 방향에 따라 바로 본다 LookAt
    protected void Trun()
    {
        transform.LookAt(transform.position + moveVec);
    }
    protected void Jump()
    {
        if(jDown && ! isJump)
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
}
