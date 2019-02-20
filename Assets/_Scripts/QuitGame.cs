using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
	public void Quit()
    {
        upnpLib upnp = new upnpLib(upnpLib.InternetConnectTypes.IPv4);
        upnp.deletePortMapping(UPDP.GetLocalIP(), UPDP.eport.ToString(), "UDP");
        Debug.Log("Server Closed!");
        Application.Quit();
    }
}
