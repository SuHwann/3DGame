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
    private void Awake()
    {
        GetPoint();
        VariableRest();
        randomInt = Random.Range(0, movePoint.Length); //���� ���� ����
        dePosition = transform.position; //ù���۽� ��ġ ����
        StartCoroutine(AiMonster());
    }
    void GetPoint()  //Point���� ��ġ�� �ν����Ϳ� �����Ѵ�.
    {
        movePoint = GameObject.Find("Point").GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position , Player.position)< 3f)
        {
            agent.SetDestination(Player.position);
        }
    }
    IEnumerator AiMonster()
    {
        while(true)
        {
            agent.SetDestination(movePoint[randomInt].transform.position); //ai���� �������� �̵� ����
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
        /*        else if(other.tag == "Bullet")
                {
                    Bullet bullet = other.GetComponent<Bullet>();
                    curHealth -= bullet.damage;

                    StartCoroutine(OnDamage(reactVec));
                }*/
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
            mat.color = Color.gray;
            gameObject.layer = 7;
            //���Ͱ� �����鼭 �ڷ� �и�
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4f);
        }
    }
}
