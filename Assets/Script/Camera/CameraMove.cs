using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform objectTofollw; //���󰡾��� ������Ʈ ��ġ ����
    public float followSpeed = 10f;       //���� ���ǵ�
    public float sensitivity = 100f;        //���콺 ���� 
    public float clampAngle = 70f;  //���콺�� ������ ��� ���� 
    //���콺 Input�� ���� ����
    private float rotx;
    private float roty;

    public Transform realCamera;        //ī�޶��� ����
    public Vector3 dirNormalized;       //ī�޶� ���� 
    public Vector3 finalDir;            //���������� ������ ����
    public float minDistance;           //�ּ� �Ÿ� 
    public float maxDistance;           //�ִ� �Ÿ� 
    public float finalDistatnce;        //���� �Ÿ� 
    public float smoothness = 10f;
    private void Start() //���� �ʱ� ȭ
    {
        rotx = transform.localRotation.eulerAngles.x;
        roty = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized;
        finalDistatnce = realCamera.localPosition.magnitude;
    }
    private void Update()
    {
        rotx += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime; //rotx�ε� �� y�� input�� ������? y�� �������� �¿�� �����̱� ����
        roty += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotx = Mathf.Clamp(rotx,-clampAngle , clampAngle);              //�����Լ� ���� Mathf�� ���
        Quaternion rot = Quaternion.Euler(rotx, roty, 0);
        transform.rotation = rot;
    }
    private void LateUpdate() //Update�� ���� ������ ����Ǵ� �Լ��̴�.
    {
        transform.position = Vector3.MoveTowards(transform.position, objectTofollw.position,
        followSpeed * Time.deltaTime);
        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position,finalDir,out hit))
        {
            finalDistatnce = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistatnce = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistatnce, Time.deltaTime * smoothness);
            
    }
}
/*    [SerializeField]
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
*/