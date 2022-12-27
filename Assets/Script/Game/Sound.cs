using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    #region 변수
    AudioSource audioSource;
    [Header("범용 공통 사운드")]
    [SerializeField]
    AudioClip[] clips;
    #endregion
    #region 함수
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //사운드 플레이중일땐 실행 X
    public void SoundByNum(int num)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clips[num]);
        }
    }
    //연속 사운드
    public void SoundByNum2(int num)
    {
        audioSource.PlayOneShot(clips[num]);
    }
    //사운드 플레이
    public void SoundByClip(AudioClip clip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    //볼륨조절
    public void SetVolume(float num)
    {
        audioSource.volume = num;
    }
    #endregion
}
