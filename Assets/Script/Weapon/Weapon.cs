using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    //무기타입 
    public enum Type { Melee, Blue}
    //무기옵션 변수
    public Type type;
    public int damage;
    public float rate;
    [SerializeField]
    private BoxCollider meleeArea;
    /*    [SerializeField]
        private TrailRenderer trailEffect;*/
    public Transform slashPos;      //참격생성 위치  
    public GameObject slash;        //참격 오브젝트
    //플레이어 무기 사용 
    public void Use()
    {
        if(type == Type.Melee )
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type == Type.Blue)
        {
            slash.SetActive(true);
            StartCoroutine(SlashShot());
        }
    }
    //근접 공격 
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
/*        trailEffect.enabled = true;*/
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.3f);
/*        trailEffect.enabled = false;*/
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
}
