using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public static PlayerBase instance;

    //0 : left 1: right
    public GunBase[] gun;
    public Aim[] hand;
    public PlayerStatus status;
    public OVRScreenFade bloodEffect;
    public GameObject centerCamera;
    AudioSource audioSource;
    public bool canEffect = true;

    private float maxHP = 100;
    private const float VIBRATION_TIME = 0.2f;
    private const float VIBRATION_FREQUENCY = 10F;
    private const float VIBRATION_AMPLITUDE = 2F;
    private const float EFFECT_TIME = 1.5F;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if ((ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch)||
             ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch)||
             ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch)||
             ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch))
             && GameManager.Instance.CheckPlayingGame() && Aim.isChooseTower == false) 
        {
            if(GameManager.Instance.playerState == PlayerState.PLAY)
            {
                GameManager.Instance.playerState = PlayerState.SHOP; 
                KHJUIManager.Instance.OpenShop();
                ChangeHand(true);

                //SHOP UI로 넘어감
            }
            else if (GameManager.Instance.playerState == PlayerState.SHOP)
            {
                GameManager.Instance.playerState = PlayerState.PLAY; 
                KHJUIManager.Instance.CloseShop();
                ChangeHand(false);

                //PLAY UI로 넘어감
            }
        }


    }

    public void ChangeHand(bool isHand)
    {
        gun[0].gameObject.SetActive(!isHand);
        gun[1].gameObject.SetActive(!isHand);
        if (!isHand)
        {
            gun[0].ResetSetting();
            gun[1].ResetSetting();
        }
        hand[0].gameObject.SetActive(isHand);
        hand[1].gameObject.SetActive(isHand);

    }

    private void Init()
    {
        if (instance == null)
        {
            instance = this;
        }


        audioSource = GetComponent<AudioSource>();

        InitRestart();
    }

    public void InitRestart()
    {
        maxHP = status.health;

        //PlayerStatus originStatus = Resources.Load("/"+status.name) as PlayerStatus;
        //status = Instantiate(originStatus);

        status = Instantiate(status);

        status.health = 100;
        maxHP = status.health;
        if (GameManager.Instance.playerState != PlayerState.READY)
        {
            ChangeHand(false);
        }
        //{KHJ추가
        //10.20 KHJ 변경사항 : ChangeHpText (100) -> (100, 100)으로 변경
        KHJUIManager.Instance.ChangeHpText(status.health,maxHP);
        //}KHJ 추가
        bloodEffect.transform.position = Vector3.down * -1000;
        Invoke("SetBloodEffect", 0.5f);
    }


    private void SetBloodEffect()
    {

        bloodEffect.transform.SetParent(centerCamera.transform);
        bloodEffect.transform.localPosition = Vector3.zero;
        bloodEffect.transform.localRotation = Quaternion.identity;
    }

    public void Hit(float damage)
    {
        status.health -= (int)Mathf.Round(damage);
        HitReaction();
        if (status.health <= 0)
        {
            Invoke("Die", 2);
        }
    }

    private void Die()
    {
        ChangeHand(true);
        GameManager.Instance.EndGame();
        //게임오버 ui

        //이 밑으로 
        KHJUIManager.Instance.OnGameOver();
    }

    private void HitReaction()
    {
        //피격처리
        ARAVRInput.PlayVibration(VIBRATION_TIME, VIBRATION_FREQUENCY, VIBRATION_AMPLITUDE, ARAVRInput.Controller.RTouch);
        ARAVRInput.PlayVibration(VIBRATION_TIME, VIBRATION_FREQUENCY, VIBRATION_AMPLITUDE, ARAVRInput.Controller.LTouch);
        if(audioSource!=null && audioSource.clip!=null)
        {
            audioSource.Play();
        }
        if (canEffect)
        {
            StartCoroutine(DelayEffectRoutine());
            bloodEffect.fadeTime = EFFECT_TIME;
            bloodEffect.FadeIn();
        }
        KHJUIManager.Instance?.ChangeHpText(status.health, maxHP);
    }

    IEnumerator DelayEffectRoutine()
    {
        canEffect = false;
        yield return new WaitForSeconds(EFFECT_TIME);
        canEffect = true;
    }

}
