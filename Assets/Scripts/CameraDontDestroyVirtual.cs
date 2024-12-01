using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDontDestroyVirtual : Singleton<CameraDontDestroyVirtual>
{
    protected override void Awake()
    {
        base.Awake();
    }
}
