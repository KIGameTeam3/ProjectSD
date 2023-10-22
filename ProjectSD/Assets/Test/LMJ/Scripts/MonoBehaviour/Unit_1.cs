using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_1 : UnitBase
{
    private void OnDestroy()
    {
        KHJUIManager.Instance.PopUpMsg("unit1 Destroy");
    }
}
