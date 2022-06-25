using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LocalPlayerSaver : NetworkBehaviour
{
    void Start()
    {
        if (!isLocalPlayer) return;
        gameObject.name = "LocalPlayerObject";
    }
}
