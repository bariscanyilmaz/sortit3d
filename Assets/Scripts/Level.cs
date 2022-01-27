using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Tube[] _tubes;

    public bool CheckWin()
    {

        foreach (var item in _tubes)
        {
            if (!item.IsRightOrder()) return false;
        }

        return true;
    }
}
