using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    const int PLAYER_START_GOLD = 100;

    //싱글턴으로 관리한다.
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }

        private set { instance = value; }
    }


    public int gold = 0;        //플레이어 골드
    private float StartTime = 0;      //진행시간
    PlayerState playerState = PlayerState.READY;    //현재 게임 상태

    public float GetPlayTime()
    {
        return Time.time - StartTime;
    }


    public void StartGame()
    {
        InitManager();
    }

    private void InitManager()
    {
        StartTime = Time.time;
        gold = PLAYER_START_GOLD;
    }

}
enum PlayerState
{
    READY, START, SHOP, DEAD
}