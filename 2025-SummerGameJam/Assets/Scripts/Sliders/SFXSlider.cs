using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    private SoundManager soundManager;

    void Start()
    {
        // SoundManager 인스턴스를 찾음
        soundManager = FindObjectOfType<SoundManager>();

        // 저장된 슬라이더 값 불러오기
        float savedVolume = PlayerPrefs.GetFloat("effectVolume", 1.0f); // 기본값은 1.0f
        if (soundManager != null && sfxSlider != null)
        {
            sfxSlider.value = savedVolume; // 저장된 값으로 슬라이더 초기화
            sfxSlider.onValueChanged.AddListener(SoundManager.Instance.OnEffectVolumeChange);
        }

        // 효과음 볼륨 설정
        //SetSFXVolume(savedVolume);
    }

    //void SetSFXVolume(float value)
    //{
    //    if (soundManager != null)
    //    {
    //        soundManager.SetEffectVolume(value); // SoundManager의 효과음 볼륨 설정
    //        PlayerPrefs.SetFloat("SFXVolume", value); // 볼륨 값 저장
    //        PlayerPrefs.Save(); // 저장된 데이터 즉시 적용
    //    }
    //}
}
