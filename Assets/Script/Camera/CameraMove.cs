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

    public float xmove = 2; // X�� ���� �̵��� 
    public float ymove = 25; // Y�� ���� �̵���
    private void Awake()
    {
        //ī�޶� �ڱ� �ڽ� transform��ġ ���� offset�� ����
        offset = GetComponent<Transform>().transform.position;
    }
    private void Update()
    {
        //ī�޶� tartget�� ���󰡰Բ� ����
        transform.position = target.position + offset;
        if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X");
            // ���콺�� �¿� �̵����� xmove �� �����մϴ�. 
            ymove -= Input.GetAxis("Mouse Y");
            // ���콺�� ���� �̵����� ymove �� �����մϴ�. 
        }
    }

}
