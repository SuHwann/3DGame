using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Input Axis  값을 받을 전역변수 선언
    float hAxis;
    float vAxis;
    bool wDown; // walk 다운 변수
    //인스펙터 창에서 설정할 수 있도록 [SerializFild]
    [SerializeField]
    private float speed;
    Vector3 moveVec; //move 벡터

    Animator anim;//Player Anim 변수
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); //Animaltor 변수를 GetCommponentChildren()으로 초기화
    }

    private void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //Axis 값을 정수로 반환하는 함수
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//normalized 로 방향값 1로 보정

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *Time.deltaTime; //Time.deltaTime 으로 이동속도 조절

        anim.SetBool("isRun", moveVec != Vector3.zero);//SetBool() 함수로 파라메터 값을 설정
        anim.SetBool("isWalk", wDown); //walk 다운

        transform.LookAt(transform.position + moveVec); //Player 방향을 moveVec 방향으로 바라보게 한다 
    }
}
