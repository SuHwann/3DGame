using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//EnemyScript랑 합친 ai스크립트 
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
    [SerializeField]
    private BoxCollider meleeArea;      //근접 공격 박스콜라이더
    NavMeshAgent agent;       //NMA 변수
    int randomInt;            //movePoint 순서를 랜덤하게 저장 할 변수
    private bool isAttack;    //현재 공격중인지 확인하는 변수 
    Rigidbody rigid;          //충돌시 일어나는 예외 상황 방지 변수
    Material mat;             //몬스터 공격시 색변화 
    Animator anim;            //몬스터 행동 애니메이션
    Coroutine co;  //코루틴 변수  
    private void Awake()
    {
        GetPoint();
        VariableRest();
        randomInt = Random.Range(0, movePoint.Length); //랜덤 변수 저장
        co = StartCoroutine(AiMonster());
        anim.SetBool("isWalk", true);   //첫시작시 걷는 행동 실행 
    }
    void GetPoint()  //Point들의 위치를 인스펙터에 저장한다.
    {
        movePoint = GameObject.Find("Point").GetComponentsInChildren<Transform>();
    }
    IEnumerator AiMonster()    //몬스터 정찰 기능 
    { 
         while (agent.SetDestination(movePoint[randomInt].transform.position))//랜덤하게 목적지로 이동 시작 
         {
            if (Vector3.Distance(transform.position, Player.position) <= 20f) //Player와 가까워지면 플레이어를 따라간다.
            {
                print("Player추격");
                agent.SetDestination(Player.position);
            }
/*            if(Vector3.Distance(transform.position , Player.position) <= 5f) //Player와 매우 가까워지면 멈춤
            {
                agent.isStopped = true;
                print("정지");
            }*/
            Destination();
            yield return new WaitForSeconds(Time.deltaTime);
         }

    }
    void VariableRest() //변수 초기화.
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;    //Material은 Mesh Renderer 컴포넌트에서 접근가능!
        anim = GetComponent<Animator>();
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
        else if (other.tag == "Slash")
        {
            Slash slash = other.GetComponent<Slash>();
            curHealth -= slash.damage;
            //참격이 적과 닿았을때 삭제 되도록 
            Destroy(other.gameObject);
            //넉백 위치 or 전달
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
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
            StopCoroutine(co);
            agent.enabled = false;
            mat.color = Color.gray;
            gameObject.layer = 7;
            anim.SetTrigger("doDie");
            //몬스터가 죽으면서 뒤로 밀림
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 3f);
        }
    }
    //Player에게 Ray를 쏜다
    void Targerting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits =
             Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
             LayerMask.GetMask("Player"));
        //rayHit 변수에 데이터가 들어오면 공격 코루틴 실행 , 이미 공격중일땐 실행 안됨
        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }
    //몬스터 공격기능 BoxCollider false true로 공격 전달
    IEnumerator Attack()
    {
        agent.isStopped = true;
        isAttack = true;
        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        agent.isStopped = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }
    private void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();
    }
}
