using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    //����Ÿ�� 
    public enum Type { Melee, Range}
    //����ɼ� ����
    [SerializeField]
    private Type type;
    public int damage;
    public float rate;
    [SerializeField]
    private BoxCollider meleeArea;
/*    [SerializeField]
    private TrailRenderer trailEffect;*/

    //�÷��̾� ���� ��� 
    public void Use()
    {
        if(type == Type.Melee )
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }
    //���� ���� 
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
