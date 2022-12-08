using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField]
    public int damage;
    [SerializeField]
    bool isMelee;       //근접 공격

    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.CompareTag("Floor")) //근접이 아니고 Floor일경우에만 삭제 
        {
            Destroy(gameObject);
        }
    }
}
