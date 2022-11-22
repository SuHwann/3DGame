using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int maxHealth; //�ִ� ü��
    [SerializeField]
    private int curHealth; //���� ü�� 
    [SerializeField]
    private bool isChase;  //Player�� ������ �����ϴ� ���� 
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    Animator anim;
    NavMeshAgent nav;
    GameObject target;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;    //Material�� Mesh Renderer ������Ʈ���� ���ٰ���!
        anim = GetComponent<Animator>();

        Invoke("ChaseStart", 2);
    }
    //Walk Anim ��ŸƮ �Լ�
    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }
    private void Update()
    {
        //isChase�� ture�̸� Player�� �����Ѵ�.
        if(isChase)
        {
            nav.SetDestination(target.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //�ٰŸ� �����϶� 
        if(other.tag == "Melee")
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
            //���Ͱ� �״� ������ �ִϸ��̼ǰ� �÷��� ����
            nav.enabled = false;
            anim.SetTrigger("doDie");
            //���Ͱ� �����鼭 �ڷ� �и�
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4f);
        }
    }
}
