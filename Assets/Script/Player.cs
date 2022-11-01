using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Input Axis  ���� ���� �������� ����
    float hAxis;
    float vAxis;
    bool wDown; // walk �ٿ� ����
    //�ν����� â���� ������ �� �ֵ��� [SerializFild]
    [SerializeField]
    private float speed;
    Vector3 moveVec; //move ����

    Animator anim;//Player Anim ����
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); //Animaltor ������ GetCommponentChildren()���� �ʱ�ȭ
    }

    private void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //Axis ���� ������ ��ȯ�ϴ� �Լ�
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;//normalized �� ���Ⱚ 1�� ����

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *Time.deltaTime; //Time.deltaTime ���� �̵��ӵ� ����

        anim.SetBool("isRun", moveVec != Vector3.zero);//SetBool() �Լ��� �Ķ���� ���� ����
        anim.SetBool("isWalk", wDown); //walk �ٿ�

        transform.LookAt(transform.position + moveVec); //Player ������ moveVec �������� �ٶ󺸰� �Ѵ� 
    }
}
