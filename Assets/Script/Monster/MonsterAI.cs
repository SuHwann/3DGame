using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    private int maxHealth; //최대 체력
    [SerializeField]
    private int curHealth; //현재 체력 
    [SerializeField]
    Transform[] movePoint;    //몬스터 목표 위치 변수
    [SerializeField]
    private Transform Player;         //몬스터 추적 Target변수
    NavMeshAgent agent;       //NMA 변수
    Vector3 dePosition;       //기존 위치 
    int randomInt;            //movePoint 순서를 랜덤하게 저장 할 변수
    Rigidbody rigid;          //충돌시 일어나는 예외 상황 방지 변수
    BoxCollider boxCollider;
    Material mat;             //몬스터 공격시 색변화 
    private void Awake()
    {
        GetPoint();
        VariableRest();
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
    void VariableRest() //변수 초기화.
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;    //Material은 Mesh Renderer 컴포넌트에서 접근가능!
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
    private void OnTriggerEnter(Collider other)
    {
        //근거리 공격일때 
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            //넉백 위치 or 전달
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
        //원거리 공격일때 
        /*        else if(other.tag == "Bullet")
                {
                    Bullet bullet = other.GetComponent<Bullet>();
                    curHealth -= bullet.damage;

                    StartCoroutine(OnDamage(reactVec));
                }*/
    }
    //데미지 로직 
    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        //몬스터 살아있음 
        if (curHealth > 0)
        {
            mat.color = Color.white;
        }
        //몬스터 죽음 
        else
        {
            mat.color = Color.gray;
            gameObject.layer = 7;
            //몬스터가 죽으면서 뒤로 밀림
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4f);
        }
    }
}
