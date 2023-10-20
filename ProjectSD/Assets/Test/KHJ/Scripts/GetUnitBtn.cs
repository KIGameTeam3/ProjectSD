using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUnitBtn : MonoBehaviour
{
    public GameObject[] unitBtnList = default;

    private float originWidth, originHeight;
    private RectTransform parent;
    private GridLayoutGroup grid;

    //[SerializeField] private GameObject btnPrefab;
    
    void Awake()
    {
        unitBtnList = Resources.LoadAll<GameObject>("UnitBtnPrefabs/");
    }
    private void OnEnable()
    {
        Debug.Log("버튼 생성기 켜지나요?");
        //CreateUnitBtn();
    }
    private void OnDisable()
    {
        //CreateUnitBtn();
    }
    void Start()
    {
        Debug.Log($"{unitBtnList.Length}");
        
        CreateUnitBtn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    //생성해야 하는 수만큼 버튼 프리팹 생성
    //그런데 얼만큼 생성해야하는지 어떻게 알지?
    public void CreateUnitBtn()
    {
        for(int i = 0; i < unitBtnList.Length; i++ )
        {
            Instantiate(unitBtnList[i],this.transform);
        }
    }

}
