using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Jewelry ,Ammo , Sword , Weapon } // 아이템 타입
    public Type type;
    public int value;             // 아이템 종류와 값을 저장할 변수 

    private void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }
}
