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
    [SerializeField]
    GameObject swapEffect;          //���� ���� ��ƼŬ
    public Transform slashPos;      //���ݻ��� ��ġ  
    public GameObject slash;        //���� ������Ʈ
    public GameObject curAmmo;      //��� ������Ʈ
    public GameObject maxcurAmmo;   //max ��� ������Ʈ
    public static Action<int> Damage; //�׼� �̺�Ʈ
    Player player;  //�÷��̾� ����
    Sound speaker;  //����Ŀ
    [SerializeField]
    Transform skillPoint; //���� ��ų ���� ����
    [SerializeField]
    GameObject ChargingOb; //��ų������Ʈ
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
                    if (Input.GetKeyDown(KeyCode.Alpha1) && player.weaponSwap[0])
                    {
                        //Green ��ų
                        swapEffect.SetActive(false);
                        player.anim.SetTrigger("doStrike");
                    }
                    break;
                case Type.Red:
                    if (Input.GetKeyDown(KeyCode.Alpha2) && player.weaponSwap[1])
                    {
                        //Red ��ų
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
    //�������� Ȱ��ȭ �ɶ����� �ڷ�ƾ ����
    private void OnEnable()
    {
        StartCoroutine(Skill());
    }
    //��¡ ���� ��ų 
    IEnumerator ChargingSkill()
    {
        player.isSkill = true;
        GameObject instantCharging = Instantiate(ChargingOb, skillPoint.position, skillPoint.rotation);
        Destroy(instantCharging,4.5f);
        yield return new WaitForSeconds(4.5f);
        player.isSkill=false;

        yield return null;
    }
}
