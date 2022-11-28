using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int maxHealth; //최대 체력
    [SerializeField]
    private int curHealth; //현재 체력 

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;    //Material은 Mesh Renderer 컴포넌트에서 접근가능!
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
