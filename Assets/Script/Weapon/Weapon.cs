using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
public class Weapon : MonoBehaviour
{

    //����Ÿ�� 
    public enum Type { Melee, Blue ,Green , Red}
    //����ɼ� ����
    public Type type;
    public int damage;
    public float rate;
    [SerializeField]
    private BoxCollider meleeArea;
    public Transform slashPos;      //���ݻ��� ��ġ  
    public GameObject slash;        //���� ������Ʈ
    public GameObject curAmmo;      //��� ������Ʈ
    public GameObject maxcurAmmo;   //max ��� ������Ʈ
    public static Action<int> Damage; //�׼� �̺�Ʈ
    private void Awake()
    {
        Damage = (int i) =>{AttackDamage(i);};
        StartCoroutine(Skill());
    }
    //�÷��̾� ���� ��� 
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

                    }
                    break;
                case Type.Red:
                    if(Input.GetKeyDown(KeyCode.Alpha2))
                    {

                    }
                    break;
                case Type.Blue:
                    if(Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        slash.SetActive(true);
                        StartCoroutine(SlashShot());
                    }
                    break;
            }
            yield return null;
        }
    }
    //���� ���� 
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
        //���� �߻� 
        GameObject instantSlash = Instantiate(slash, slashPos.position, slashPos.rotation);
        Rigidbody slashRigid = instantSlash.GetComponent<Rigidbody>();
        slashRigid.velocity = slashPos.forward * 50;
        Destroy(instantSlash,1.5f);
        yield return null;
    }
    //�÷��̾��� ���ݷ��� �޾ƿͼ� ���ϱ�
    public void AttackDamage(int Damage)
    {
        damage += Damage;
    }
}
