using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    //����Ÿ�� 
    public enum Type { Melee, Blue}
    //����ɼ� ����
    public Type type;
    public int damage;
    public float rate;
    [SerializeField]
    private BoxCollider meleeArea;
    /*    [SerializeField]
        private TrailRenderer trailEffect;*/
    public Transform slashPos;      //���ݻ��� ��ġ  
    public GameObject slash;        //���� ������Ʈ
    //�÷��̾� ���� ��� 
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
    IEnumerator SlashShot()
    {
        //���� �߻� 
        GameObject instantSlash = Instantiate(slash, slashPos.position, slashPos.rotation);
        Rigidbody slashRigid = instantSlash.GetComponent<Rigidbody>();
        slashRigid.velocity = slashPos.forward * 50;
        Destroy(instantSlash,1.5f);
        yield return null;
    }
}
