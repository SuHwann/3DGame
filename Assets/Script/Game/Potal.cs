using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Potal : MonoBehaviour
{
    [SerializeField]
    string mapname;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            print("Æ÷Å»ÀÌµ¿");
            SceneManager.LoadScene(mapname);
        }
    }
}
