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
    public Transform slashPos;      //참격생성 위치  
    public GameObject slash;        //참격 오브젝트
    public GameObject curAmmo;      //방어 오브젝트
    public GameObject maxcurAmmo;   //max 방어 오브젝트
    public static Action<int> Damage; //액션 이벤트
    Player player;  //플레이어 변수
    Sound speaker;  //스피커
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        speaker= FindObjectOfType<Sound>();
        Damage = (int i) =>{AttackDamage(i);};
        StartCoroutine(Skill());
    }
    //플레이어 무기 사용 
    public void Use()
    {
        if(type == Type.Melee )
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }
    }
    IEnumerator Skill()
    {
        while(true)
        {
            switch (type)
            {
                case Type.Green:
                    if(Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        //Green 스킬
                    }
                    break;
                case Type.Red:
                    if(Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        //Red 스킬
                    }
                    break;
                case Type.Blue:
                    if (Input.GetKeyDown(KeyCode.Alpha3)/* && player.weaponSwap[2]*/)
                    {
                        print("안되는 이유는?");
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
}
