using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    //무기타입 
    public enum Type { Melee, Range}
    //무기옵션 변수
    [SerializeField]
    private Type type;
    public int damage;
    public float rate;
    [SerializeField]
    private BoxCollider meleeArea;
/*    [SerializeField]
    private TrailRenderer trailEffect;*/

    //플레이어 무기 사용 
    public void Use()
    {
        if(type == Type.Melee )
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
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
}
