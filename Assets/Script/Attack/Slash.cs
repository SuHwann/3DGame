using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField]
    public int damage;
    [SerializeField]
    bool isMelee;       //���� ����

    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.CompareTag("Floor")) //������ �ƴϰ� Floor�ϰ�쿡�� ���� 
        {
            Destroy(gameObject);
        }
    }
}
