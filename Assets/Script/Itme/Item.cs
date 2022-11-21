using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Item : MonoBehaviour
{
    public enum Type { Jewelry ,Ammo , Sword , Weapon ,Coin , Grenade , Heart } // 아이템 타입
    public Type type;
    public int value;             // 아이템 종류와 값을 저장할 변수 

    Rigidbody rigid;
    SphereCollider sphereCollider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();//초기화 
        sphereCollider = GetComponent<SphereCollider>();//초기화 
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime); //아이템 회전
    }
    //아이템 물리충돌 제거 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
