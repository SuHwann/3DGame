using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int maxHealth; //최대 체력
    [SerializeField]
    private int curHealth; //현재 체력 
    [SerializeField]
    private bool isChase;  //Player를 추적을 결정하는 변수 
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
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;    //Material은 Mesh Renderer 컴포넌트에서 접근가능!
        anim = GetComponent<Animator>();

        Invoke("ChaseStart", 2);
    }
    //Walk Anim 스타트 함수
    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }
    private void Update()
    {
        //isChase가 ture이면 Player를 추적한다.
        if(isChase)
        {
            nav.SetDestination(target.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //근거리 공격일때 
        if(other.tag == "Melee")
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
            //몬스터가 죽는 시점에 애니메이션과 플래그 셋팅
            nav.enabled = false;
            anim.SetTrigger("doDie");
            //몬스터가 죽으면서 뒤로 밀림
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            Destroy(gameObject, 4f);
        }
    }
}
