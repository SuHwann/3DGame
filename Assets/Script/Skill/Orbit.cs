using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float orbitSpeed;
    Vector3 offset;

    private void Awake()
    {
        offset = transform.position - target.position;
        StartCoroutine(OrbitSkill());
    }
    //Effect 공전 함수 
    IEnumerator OrbitSkill()
    {
        while (true)
        {
            transform.position = target.position + offset;
            transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
            offset = transform.position - target.position;
            yield return null; 
        }
    }
}
