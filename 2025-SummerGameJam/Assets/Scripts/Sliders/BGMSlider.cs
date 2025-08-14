using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    private SoundManager soundManager;

    void Start()
    {
        // SoundManager 인스턴스를 찾음
        soundManager = FindObjectOfType<SoundManager>();

        // 저장된 슬라이더 값 불러오기
        float savedVolume = PlayerPrefs.GetFloat("bgmVolume", 1.0f); // 기본값은 1.0f
        if (soundManager != null && bgmSlider != null)
        {
            bgmSlider.value = savedVolume; // 저장된 값으로 슬라이더 초기화
            bgmSlider.onValueChanged.AddListener(SoundManager.Instance.OnBgmVolumeChange);
        }

        // 배경음악 볼륨 설정
        //SetBGMVolume(savedVolume);
    }

    //void SetBGMVolume(float value)
    //{
    //    if (soundManager != null)
    //    {
    //        soundManager.SetBGMVolume(value);
    //        PlayerPrefs.SetFloat("BGMVolume", value); // 볼륨 값 저장
    //        PlayerPrefs.Save(); // 저장된 데이터 즉시 적용
    //    }
    //}
}