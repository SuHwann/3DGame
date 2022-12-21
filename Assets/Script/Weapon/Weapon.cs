using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    //무기타입 
    public enum Type { Melee, Blue ,Green , Red}
    //무기옵션 변수
    public Type type;
    public int damage;
    public float rate;
    [SerializeField]
    private BoxCollider meleeArea;
    [SerializeField]
    GameObject swapEffect;          //스왑 번개 파티클
    public Transform slashPos;      //참격생성 위치  
    public GameObject slash;        //참격 오브젝트
    public static Action<int> Damage; //액션 이벤트
    Player player;  //플레이어 변수
    Sound speaker;  //스피커
    [SerializeField]
    Transform skillPoint; //광역 스킬 시작 지점
    [SerializeField]
    GameObject ChargingOb; //스킬오브젝트
    [SerializeField]
    GameObject particle;   //스킬오브젝트
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        speaker= FindObjectOfType<Sound>();
        Damage = (int i) =>{AttackDamage(i);};
    }
    //플레이어 무기 사용 
    public void Use()
    {
       StopCoroutine(Swing());
       StartCoroutine(Swing());
    }
    IEnumerator Skill()
    {
        while (true)
        {
            switch (type)
            {
                case Type.Green:
                    if (Input.GetKeyDown(KeyCode.Alpha1) && player.weaponSwap[0])
                    {
                        //Green 스킬
                        swapEffect.SetActive(false);
                        player.anim.SetTrigger("doStrike");
                        yield return new WaitForSeconds(0.25f);
                        LightningBall();
                    }
                    break;
                case Type.Red:
                    if (Input.GetKeyDown(KeyCode.Alpha2) && player.weaponSwap[1])
                    {
                        //Red 스킬
                        swapEffect.SetActive(false);
                        player.anim.SetTrigger("doExplosion");
                        StartCoroutine(ChargingSkill());
                    }
                    break;
                case Type.Blue:
                    if (Input.GetKeyDown(KeyCode.Alpha3) && player.weaponSwap[2])
                    {
                        swapEffect.SetActive(false);
                        player.anim.SetTrigger("doSwing");
                        speaker.SoundByNum2(7);
                        slash.SetActive(true);
                        StartCoroutine(SlashShot());
                    }
                    break;

            }
            yield return null;
        }
    }
    //근접 공격 
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.3f);
    }
    IEnumerator SlashShot()
    {
        //참격 발사 
        GameObject instantSlash = Instantiate(slash, slashPos.position, slashPos.rotation);
        Rigidbody slashRigid = instantSlash.GetComponent<Rigidbody>();
        slashRigid.velocity = slashPos.forward * 50;
        Destroy(instantSlash,1.5f);
        yield return null;
    }
    //플레이어의 공격력을 받아와서 더하기
    public void AttackDamage(int Damage)
    {
        damage += Damage;
    }
    //아이템이 활성화 될때마다 코루틴 실행
    private void OnEnable()
    {
        StartCoroutine(Skill());
    }
    //차징 광역 스킬 
    IEnumerator ChargingSkill()
    {
        player.isSkill = true;
        GameObject instantCharging = Instantiate(ChargingOb, skillPoint.position, skillPoint.rotation);
        Destroy(instantCharging,4.5f);
        yield return new WaitForSeconds(5f);
        player.isSkill=false;
        yield return null;
    }
    //초록 번개 스킬
    private void LightningBall()
    {
        GameObject instantLightning = Instantiate(slash , skillPoint.position,skillPoint.rotation);
        GameObject instantParticle = Instantiate(particle, skillPoint.position, skillPoint.rotation);
        Rigidbody slashRigid = instantLightning.GetComponent<Rigidbody>();
        slashRigid.velocity = player.transform.forward * 50;
        Destroy(instantLightning, 1f);
        Destroy(instantParticle, 1f);
    }
}
