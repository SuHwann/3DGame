using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField]
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3f);
        }
        else if(collision.gameObject.tag =="Wall")
        {
            Destroy(gameObject);
        }
    }
}
