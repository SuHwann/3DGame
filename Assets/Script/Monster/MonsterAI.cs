using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//EnemyScript�� ��ģ ai��ũ��Ʈ 
public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    private int maxHealth; //�ִ� ü��
    [SerializeField]
    private int curHealth; //���� ü�� 
    [SerializeField]
    Transform[] movePoint;    //���� ��ǥ ��ġ ����
    [SerializeField]
    private Transform Player;         //���� ���� Target����
    [SerializeField]
    private BoxCollider meleeArea;      //���� ���� �ڽ��ݶ��̴�
    NavMeshAgent agent;       //NMA ����
    int randomInt;            //movePoint ������ �����ϰ� ���� �� ����
    private bool isAttack;    //���� ���������� Ȯ���ϴ� ���� 
    Rigidbody rigid;          //�浹�� �Ͼ�� ���� ��Ȳ ���� ����
    Material mat;             //���� ���ݽ� ����ȭ 
    Animator anim;            //���� �ൿ �ִϸ��̼�
    Coroutine co;  //�ڷ�ƾ ����  
    private void Awake()
    {
        GetPoint();
        VariableRest();
        randomInt = Random.Range(0, movePoint.Length); //���� ���� ����
        co = StartCoroutine(AiMonster());
        anim.SetBool("isWalk", true);   //ù���۽� �ȴ� �ൿ ���� 
    }
    void GetPoint()  //Point���� ��ġ�� �ν����Ϳ� �����Ѵ�.
    {
        movePoint = GameObject.Find("Point").GetComponentsInChildren<Transform>();
    }
    IEnumerator AiMonster()    //���� ���� ��� 
    { 
         while (agent.SetDestination(movePoint[randomInt].transform.position))//�����ϰ� �������� �̵� ���� 
         {
            if (Vector3.Distance(transform.position, Player.position) <= 20f) //Player�� ��������� �÷��̾ ���󰣴�.
            {
                print("Player�߰�");
                agent.SetDestination(Player.position);
            }
/*            if(Vector3.Distance(transform.position , Player.position) <= 5f) //Player�� �ſ� ��������� ����
            {
                agent.isStopped = true;
                print("����");
            }*/
            Destination();
            yield return new WaitForSeconds(Time.deltaTime);
         }

    }
    void VariableRest() //���� �ʱ�ȭ.
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;    //Material�� Mesh Renderer ������Ʈ���� ���ٰ���!
        anim = GetComponent<Animator>();
    }
    void FreezeVelocity()   //ai ���� �浹�� �ڷ� �и��� �浹 ���� 
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    void Destination() //�������� �����ϸ� ������ ����
    {
        if (Vector3.Distance(transform.position, movePoint[randomInt].transform.position) < 1f)
        {
            randomInt = Random.Range(0, movePoint.Length);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //�ٰŸ� �����϶� 
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            //�˹� ��ġ or ����
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
        //���Ÿ� �����϶� 
        else if (other.tag == "Slash")
        {
            Slash slash = other.GetComponent<Slash>();
            curHealth -= slash.damage;
            //������ ���� ������� ���� �ǵ��� 
            Destroy(other.gameObject);
            //�˹� ��ġ or ����
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
    }
    //������ ���� 
    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        //���� ������� 
        if (curHealth > 0)
        {
            mat.color = Color.white;
        }
        //���� ���� 
        else
        {
            StopCoroutine(co);
            agent.enabled = false;
            mat.color = Color.gray;
            gameObject.layer = 7;
            anim.SetTrigger("doDie");
            //���Ͱ� �����鼭 �ڷ� �и�
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 3f);
        }
    }
    //Player���� Ray�� ���
    void Targerting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        RaycastHit[] rayHits =
             Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
             LayerMask.GetMask("Player"));
        //rayHit ������ �����Ͱ� ������ ���� �ڷ�ƾ ���� , �̹� �������϶� ���� �ȵ�
        if(rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }
    //���� ���ݱ�� BoxCollider false true�� ���� ����
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
