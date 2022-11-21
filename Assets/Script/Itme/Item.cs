using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Item : MonoBehaviour
{
    public enum Type { Jewelry ,Ammo , Sword , Weapon ,Coin , Grenade , Heart } // ������ Ÿ��
    public Type type;
    public int value;             // ������ ������ ���� ������ ���� 

    Rigidbody rigid;
    SphereCollider sphereCollider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();//�ʱ�ȭ 
        sphereCollider = GetComponent<SphereCollider>();//�ʱ�ȭ 
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime); //������ ȸ��
    }
    //������ �����浹 ���� 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
