using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField]
    public int damage;
    [SerializeField]
    bool isMelee;       //근접 공격
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3f);
        }
        else if(!isMelee && collision.gameObject.tag =="Wall") //근접이 아니고 Wall일경우에만 삭제 
        {
            Destroy(gameObject);
        }
    }
}
