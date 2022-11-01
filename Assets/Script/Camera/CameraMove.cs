using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Vector3 offset;

    private void Update()
    {
        //카메라가 tartget을 따라가게끔 설정
        transform.position = target.position + offset;
    }
}
