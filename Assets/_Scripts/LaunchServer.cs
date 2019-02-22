using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchServer : MonoBehaviour
{
    public UPDP updp;

	public void OnClick()
    {
        updp.Create();

        int key = OptionManager.UseTest ? 1 : 0;
        Debug.Log("Server Launched with " + DLLLoader.LaunchServer(key, OptionManager.WaitInGame, OptionManager.UseBot));
    }
}
