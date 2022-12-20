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
    Player player;  //�÷��̾� ����
    Sound speaker;  //����Ŀ
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        speaker= FindObjectOfType<Sound>();
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
                        //Green ��ų
                    }
                    break;
                case Type.Red:
                    if(Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        //Red ��ų
                    }
                    break;
                case Type.Blue:
                    if (Input.GetKeyDown(KeyCode.Alpha3)/* && player.weaponSwap[2]*/)
                    {
                        print("�ȵǴ� ������?");
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
