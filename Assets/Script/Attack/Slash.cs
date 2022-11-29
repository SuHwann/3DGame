using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField]
    public int damage;
    [SerializeField]
    bool isMelee;       //���� ����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3f);
        }
        else if(!isMelee && collision.gameObject.tag =="Wall") //������ �ƴϰ� Wall�ϰ�쿡�� ���� 
        {
            Destroy(gameObject);
        }
    }
}
