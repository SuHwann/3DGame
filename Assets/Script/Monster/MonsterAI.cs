using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//EnemyScript랑 합친 ai스크립트 
public class MonsterAI : MonoBehaviour
{
    public enum Type { A , B, C , D} //몬스터 타입
    [SerializeField]
    Type enemyType; //몬스터 타입 변수 
    [SerializeField]
    private int maxHealth; //최대 체력
    [SerializeField]
    private int curHealth; //현재 체력 
    [SerializeField]
    Transform[] movePoint;    //몬스터 목표 위치 변수
    public BoxCollider meleeArea;      //근접 공격 박스콜라이더
    [SerializeField]
    private GameObject slashOb;         //원거리 공격 오브젝트
    public Transform player;         //몬스터 추적 Target변수
    public NavMeshAgent agent;       //NMA 변수
    int randomInt;            //movePoint 순서를 랜덤하게 저장 할 변수
    private bool isAttack;    //현재 공격중인지 확인하는 변수 
    public Rigidbody rigid;          //충돌시 일어나는 예외 상황 방지 변수
    public Collider monsterCol;      //몬스터 콜라이더 
    public SkinnedMeshRenderer[] meshs;             //피격시 모든 메테리얼을 변경
    public Animator anim;            //몬스터 행동 애니메이션
    public bool isDead;               //몬스터의 죽음 체크 변수
    Coroutine co;  //코루틴 변수  
    private void Awake()
    {
        co = StartCoroutine(AiMonster());
        GetPoint();
        VariableRest();
        randomInt = Random.Range(0, movePoint.Length); //랜덤 변수 저장
        StartCoroutine(Attack());
        anim.SetBool("isWalk", true);   //첫시작시 걷는 행동 실행 
    }
    void GetPoint()  //Point들의 위치를 인스펙터에 저장한다.
    {
        movePoint = GameObject.Find("Point").GetComponentsInChildren<Transform>();
    }
    IEnumerator AiMonster()    //몬스터 정찰 기능 
    { 
        if(enemyType != Type.D) //보스몬스터가 아니면 실행
        {
            while (agent.SetDestination(movePoint[randomInt].transform.position))//랜덤하게 목적지로 이동 시작 
            {
                if (Vector3.Distance(transform.position, player.position) <= 20f) //Player와 가까워지면 플레이어를 따라간다.
                {
                    agent.SetDestination(player.position);
                }
                /*            if(Vector3.Distance(transform.position , player.position) <= 5f) //Player와 매우 가까워지면 멈춤
                            {
                                agent.isStopped = true;
                                print("정지");
                            }*/
                Destination();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
    void VariableRest() //변수 초기화.
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();    //Material은 Mesh Renderer 컴포넌트에서 접근가능!
        anim = GetComponent<Animator>();
        monsterCol = GetComponent<Collider>();

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
        foreach (SkinnedMeshRenderer mesh in meshs)
        { mesh.material.color = Color.red;}
  
        yield return new WaitForSeconds(0.1f);
        //몬스터 살아있음 
        if (curHealth > 0)
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
            { mesh.material.color = Color.white; }
        }
        //몬스터 죽음 
        else
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
            { mesh.material.color = Color.gray; }
            StopAllCoroutines();
            monsterCol.enabled = false;
            gameObject.layer = 7;
            isDead = true;
            anim.SetTrigger("doDie");
            //몬스터가 죽으면서 뒤로 밀림
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            if(enemyType != Type.D) {Destroy(gameObject, 3f);} //보스가 아닐때만 삭제
        }
    }
    //Player에게 Ray를 쏜다
    void Targerting()
    {
        if(!isDead && enemyType != Type.D)
        {
            float targetRadius = 0f;
            float targetRange = 0f;
            //몬스터 타입 분기문
            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 3f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits =
                 Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
                 LayerMask.GetMask("Player"));
            //rayHit 변수에 데이터가 들어오면 공격 코루틴 실행 , 이미 공격중일땐 실행 안됨
            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
       
    }
    //몬스터 공격기능 BoxCollider false true로 공격 전달
    IEnumerator Attack()
    {
        agent.isStopped = true;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A: //일반 몬스터 행동 
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B: //돌격형 몬스터 행동
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20 , ForceMode.Impulse); //돌격 구현 
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero; //속도 제어 
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z);
                GameObject instantSlash = Instantiate(slashOb, pos, transform.rotation);
                Rigidbody rigidSlash = instantSlash.GetComponent<Rigidbody>();
                rigidSlash.velocity = transform.forward * 20f;

                yield return new WaitForSeconds(2f);
                Destroy(instantSlash, 1f);
                break;
        }
        agent.isStopped = false;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }
    private void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();
    }
}
