using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class ETFXProjectileScript : MonoBehaviour
    {
        public GameObject impactParticle; //발사체가 충돌체에 부딪힐 때 생성되는 효과
        public GameObject projectileParticle; // 자식으로 gameobject에 첨부된 효과
        public GameObject muzzleParticle; // 게임 오브젝트가 생성될 때 즉시 생성되는 효과
                [Header("Adjust if not using Sphere Collider")]
        public float colliderRadius = 1f;
        [Range(0f, 1f)] // 이것은 충격 효과의 클리핑을 줄이기 위해 충격 효과를 충격 지점에서 약간 멀리 이동시키는 오프셋입니다.
        public float collideOffset = 0.15f;

        void Start()
        {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.parent = transform;
            if (muzzleParticle)
            {
                muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
                Destroy(muzzleParticle, 1.5f); // 두 번째 매개변수는 효과의 수명(초)입니다.
            }
        }
		
        void FixedUpdate()
        {	
			if (GetComponent<Rigidbody>().velocity.magnitude != 0)
			{
			    transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity); //이동 방향을 보도록 회전을 설정합니다.
			}
			
            RaycastHit hit;
			
            float radius; //충돌 감지 반경 설정
            if (transform.GetComponent<SphereCollider>())
                radius = transform.GetComponent<SphereCollider>().radius;
            else
                radius = colliderRadius;

            Vector3 direction = transform.GetComponent<Rigidbody>().velocity; //충돌 감지에 사용되는 발사체의 방향을 가져옵니다.
            if (transform.GetComponent<Rigidbody>().useGravity)
                direction += Physics.gravity * Time.deltaTime; //  활성화된 경우 중력을 고려합니다.
            direction = direction.normalized;

            float detectionDistance = transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime; //이 프레임에 대한 충돌 감지 거리

            if (Physics.SphereCast(transform.position, radius, direction, out hit, detectionDistance)) //충돌이 발생하는지 확인
            {
                transform.position = hit.point + (hit.normal * collideOffset); //발사체를 충돌 지점으로 이동

                GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; // 충격 효과 생성

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();/* 트레일을 분리해야 하므로 파티클 시스템 목록을 가져옵니다.*/
                //[0]의 구성 요소는 부모 즉, 이 개체(있는 경우)의 구성 요소입니다.
                for (int i = 1; i < trails.Length; i++)      // 찾은 입자 시스템을 순환하는 루프
                {
                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);// 발사체에서 트레일을 분리합니다.
                        Destroy(trail.gameObject, 2f);  // 초 후 트레일을 제거합니다.
                    }
                }
                Destroy(projectileParticle, 3f); // 지연 후 파티클 효과 제거
                Destroy(impactP, 3.5f); // 지연 후 충격 효과 제거
                Destroy(gameObject); // 발사체 제거
            }
        }
    }
}