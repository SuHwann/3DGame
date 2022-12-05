using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform objectTofollow;   //ī�޶� �ٶ� �÷��̾� �߽�
    [SerializeField] 
    Transform realCamera;               //mainCamera
    public float followSpeed = 10f;     //ī�޶� ����ٴ� �ӵ� 
    private Vector3 dirNoramlized;      
    private Vector3 finalDir;           //���� �Ÿ� 
    public float minDistance;           //�÷��̾���� �ּ� �Ÿ�
    public float maxDistance;           //�÷��̾���� �ִ� �Ÿ�
    public float finalDistance;         //���� �Ÿ�     
    public float smoothness = 10f;      //����ٴ� �ӵ��� �󸶳� �ε巴�� ���ٰ��ΰ�
    private void Start()
    {
        dirNoramlized = realCamera.localPosition.normalized;    //����ȭ
        finalDistance = realCamera.localPosition.magnitude;     //�Ÿ��ʱ�ȭ
        StartCoroutine(CameraMove());
    }
    //�÷��̾ ��ǥ�� ī�޶� ����ٴ�
    IEnumerator CameraMove()
    {
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime); //�÷��̾� �̵� 
            finalDir = transform.TransformPoint(dirNoramlized * maxDistance);
            RaycastHit hit;
            if (Physics.Linecast(transform.position, finalDir, out hit))
            {
                finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
            else { finalDistance = maxDistance; }
            realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNoramlized * finalDistance, Time.deltaTime * smoothness);
            yield return null;
        }
    }
}
