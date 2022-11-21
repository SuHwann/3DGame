using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    Transform[] movePoint;    //몬스터 목표 위치 변수
    NavMeshAgent agent;       //NMA 변수
    Vector3 dePosition;       //기존 위치 
    int randomInt;            //movePoint 순서를 랜덤하게 저장 할 변수
    Rigidbody rigid;          //충돌시 일어나는 예외 상황 방지 변수
    private void Awake()
    {
        GetPoint();
        randomInt = Random.Range(0, movePoint.Length); //랜덤 변수 저장
        dePosition = transform.position; //첫시작시 위치 저장
        StartCoroutine(AiMonster());
    }
    void GetPoint()  //Point들의 위치를 인스펙터에 저장한다.
    {
        movePoint = GameObject.Find("Point").GetComponentsInChildren<Transform>();
    }
    IEnumerator AiMonster()
    {
        while(true)
        {
            agent.SetDestination(movePoint[randomInt].transform.position); //ai몬스터 목적지로 이동 시작
            FreezeVelocity();
            Destination();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    void FreezeVelocity()   //ai 몬스터 충돌시 뒤로 밀리는 충돌 멈춤 
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    void Destination() //목적지에 도착하면 목적지 변경
    {
        if (Vector3.Distance(transform.position, movePoint[randomInt].transform.position) < 1f)
        {
            randomInt = Random.Range(0, movePoint.Length);
        }
    }
}
