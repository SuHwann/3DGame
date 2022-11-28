using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    NavMeshAgent agent;       //NMA ����
    Vector3 dePosition;       //���� ��ġ 
    int randomInt;            //movePoint ������ �����ϰ� ���� �� ����
    Rigidbody rigid;          //�浹�� �Ͼ�� ���� ��Ȳ ���� ����
    BoxCollider boxCollider;
    Material mat;             //���� ���ݽ� ����ȭ 
    Animator anim;            //���� �ൿ �ִϸ��̼�
    Coroutine co;  //�ڷ�ƾ ���� 
    bool isChase = true;              //���� �ൿ ���� 
    private void Awake()
    {
        GetPoint();
        VariableRest();
        randomInt = Random.Range(0, movePoint.Length); //���� ���� ����
        dePosition = transform.position; //ù���۽� ��ġ ���� .     ���߿� ������ �� ��Ȳ�� ��������ֱ� ������ 
        co = StartCoroutine(AiMonster());
        anim.SetBool("isWalk", true);   //ù���۽� �ȴ� �ൿ ���� 
    }
    void GetPoint()  //Point���� ��ġ�� �ν����Ϳ� �����Ѵ�.
    {
        movePoint = GameObject.Find("Point").GetComponentsInChildren<Transform>();
    }
    IEnumerator AiMonster()    //���� ���� ��� 
    { 
         while (true)
         {
            agent.SetDestination(movePoint[randomInt].transform.position); //ai���� �������� �̵� ����
            if (Vector3.Distance(transform.position, Player.position) <= 20f) //Player�� ��������� �÷��̾ ���󰣴�.
            {
                agent.SetDestination(Player.position);
            }
            FreezeVelocity();
            Destination();
            yield return new WaitForSeconds(Time.deltaTime);
         }

    }
    void VariableRest() //���� �ʱ�ȭ.
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
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
}
