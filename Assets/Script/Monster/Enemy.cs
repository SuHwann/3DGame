using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int maxHealth; //�ִ� ü��
    [SerializeField]
    private int curHealth; //���� ü�� 

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;    //Material�� Mesh Renderer ������Ʈ���� ���ٰ���!
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
