using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform objectTofollw; //따라가야할 오브젝트 위치 정보
    public float followSpeed = 10f;       //따라갈 스피드
    public float sensitivity = 100f;        //마우스 감도 
    public float clampAngle = 70f;  //마우스를 움직때 상수 각도 
    //마우스 Input을 받을 정보
    private float rotx;
    private float roty;

    public Transform realCamera;        //카메라의 정보
    public Vector3 dirNormalized;       //카메라 방향 
    public Vector3 finalDir;            //최종적으로 정해진 방향
    public float minDistance;           //최소 거리 
    public float maxDistance;           //최대 거리 
    public float finalDistatnce;        //최종 거리 
    public float smoothness = 10f;
    private void Start() //변수 초기 화
    {
        rotx = transform.localRotation.eulerAngles.x;
        roty = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized;
        finalDistatnce = realCamera.localPosition.magnitude;
    }
    private void Update()
    {
        rotx += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime; //rotx인데 왜 y를 input을 받을까? y축 방향으로 좌우로 움직이기 때문
        roty += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotx = Mathf.Clamp(rotx,-clampAngle , clampAngle);              //수학함수 쓸때 Mathf를 사용
        Quaternion rot = Quaternion.Euler(rotx, roty, 0);
        transform.rotation = rot;
    }
    private void LateUpdate() //Update가 끝난 다음에 실행되는 함수이다.
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
*/