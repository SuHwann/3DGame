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

    public float xmove = 2; // X축 누적 이동량 
    public float ymove = 25; // Y축 누적 이동량
    private void Awake()
    {
        //카메라 자기 자신 transform위치 값을 offset에 저장
        offset = GetComponent<Transform>().transform.position;
    }
    private void Update()
    {
        //카메라가 tartget을 따라가게끔 설정
        transform.position = target.position + offset;
        if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X");
            // 마우스의 좌우 이동량을 xmove 에 누적합니다. 
            ymove -= Input.GetAxis("Mouse Y");
            // 마우스의 상하 이동량을 ymove 에 누적합니다. 
        }
    }

}
