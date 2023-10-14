using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //////////////////////////////////////////////////////////////////////////////////
        //테스트 코드
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.StartGame();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.EndGame();
        }

        //////////////////////////////////////////////////////////////////////////////////
    }
}
