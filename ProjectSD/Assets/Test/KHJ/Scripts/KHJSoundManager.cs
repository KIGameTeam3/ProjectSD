using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJSoundManager : MonoBehaviour
{
    public AudioSource myAudioSource;


    [Header("Ui Sound")]
    [SerializeField] private AudioClip enterGameSound;
    public AudioClip uiClickSound;
    public AudioClip uiVictorySound;
    public AudioClip uiDefeatSound;

    public AudioClip lobbyBgSound;

    [Header("Unit Sound")]
    public AudioClip unitSettingSound;
    public AudioClip unitDestroySound;
    //public AudioClip unitPlaySound;

    [Header("Shop")]
    public AudioClip shop_Purchase_Sound;
    public AudioClip shop_Purchase_Fail_Sound;
    public AudioClip shop_Click_Sound;


    #region 싱글톤 변수
    //싱글턴으로 관리한다.
    private static KHJSoundManager instance;
    public static KHJSoundManager Instance
    {
        get
        {
            return instance;
        }

        private set { instance = value; }
    }
    #endregion
    void Awake()
    {
        instance = this;   

        myAudioSource = GetComponent<AudioSource>();   

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSound() //TODO 나중에 사운드 매니저 생기면 옮길 예정
    {
        myAudioSource.PlayOneShot(uiClickSound);
    }

    public void EnterGameSound()
    {
        myAudioSource.PlayOneShot(enterGameSound);
    }

    public void VictoryGameSound()
    {
        myAudioSource.PlayOneShot(uiVictorySound);
    }

    public void DefeatGameSound()
    {
        myAudioSource.PlayOneShot(uiDefeatSound);
    }
}
