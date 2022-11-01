using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    Transform target;

    //ī�޶� vector����
    private Vector3 offset;

    private void Awake()
    {
        //ī�޶� �ڱ� �ڽ� transform��ġ ���� offset�� ����
        offset = GetComponent<Transform>().transform.position;
    }
    private void Update()
    {
        //ī�޶� tartget�� ���󰡰Բ� ����
        transform.position = target.position + offset;
    }
}
