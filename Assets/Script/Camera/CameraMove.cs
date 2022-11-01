using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    Transform target;

    //카메라 vector변수
    private Vector3 offset;

    private void Awake()
    {
        //카메라 자기 자신 transform위치 값을 offset에 저장
        offset = GetComponent<Transform>().transform.position;
    }
    private void Update()
    {
        //카메라가 tartget을 따라가게끔 설정
        transform.position = target.position + offset;
    }
}
