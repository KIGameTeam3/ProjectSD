using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region 상수
    const int PLAYER_START_GOLD = 100;  //시작 골드
    #endregion

    #region 싱글톤 변수
    //싱글턴으로 관리한다.
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("GameManager");
                instance = gameObject.AddComponent<GameManager>();
            }
            return instance;
        }

        private set { instance = value; }
    }
    #endregion

    #region private 변수
    private float StartTime = 0;    //시작시간
    private float EndTime = 0;      //종료시간
    #endregion

    #region public 변수
    public PlayerState playerState = PlayerState.READY;     //현재 게임 상태
    public int gold = 0;                                    //플레이어 골드
    #endregion


    //플레이 타임 불러오기
    //1. 게임중에는 진행 시간을 불러온다
    //2. 게임종료하면 버틴 시간을 불러온다.
    public float GetPlayTime()
    {
        //플레이 or 상점
        if(playerState == PlayerState.PLAY || playerState == PlayerState.SHOP)
        {
            return Time.time - StartTime;
        }
        //준비 or 종료
        else
        {
            return EndTime;
        }
    }

    //게임 시작시 불러온다
    public void StartGame()
    {
        //플레이중에는 막는다.
        if (CheckPlayingGame())
        {
            return;
        }
        InitManager();
    }

    //게임 종료시 불러온다
    public void EndGame()
    {
        //플레이중이 아니면 막는다.
        if (!CheckPlayingGame())
        {
            return;
        }
        StopManager();
    }


    //게임중인지 아닌지 판단
    private bool CheckPlayingGame()
    {
        return playerState == PlayerState.PLAY || playerState == PlayerState.SHOP;
    }

    //재시작시 내부 적용
    private void InitManager()
    {
        StartTime = Time.time;
        gold = PLAYER_START_GOLD;
        playerState = PlayerState.PLAY;
    }


    //종료시 내부 적용
    private void StopManager()
    {
        EndTime = GetPlayTime();
        gold = 0;
        playerState = PlayerState.DEAD;
    }
}

//플레이어 상태를 나타내는 enum
public enum PlayerState
{
    READY, PLAY, SHOP, DEAD
}