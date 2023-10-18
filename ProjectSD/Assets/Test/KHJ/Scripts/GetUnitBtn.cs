using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUnitBtn : MonoBehaviour
{
    [SerializeField] private GameObject btnPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        btnPrefab = Resources.Load<GameObject>("UnitBtnPrefabs/UnitBtnModel");    
    }
    private void OnEnable()
    {
        Debug.Log("버튼 생성기 켜지나요?");
        //CreateUnitBtn(4);
    }
    void Start()
    {
        CreateUnitBtn(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    //생성해야 하는 수만큼 버튼 프리팹 생성
    //그런데 얼만큼 생성해야하는지 어떻게 알지?
    public void CreateUnitBtn(int unitCount)
    {
        for(int i = 0; i < unitCount; i++ )
        {
            Instantiate(btnPrefab,this.transform);
        }
    }
}
