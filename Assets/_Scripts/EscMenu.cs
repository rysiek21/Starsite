using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class EscMenu : MonoBehaviour
{
    GameObject escCanvas;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        escCanvas = gameObject.transform.GetChild(0).gameObject;
        escCanvas.SetActive(false);
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeMenuState();
        }
    }

    public void ChangeMenuState()
    {
        Scene scene = SceneManager.GetActiveScene();
        escCanvas.SetActive(!escCanvas.activeSelf);
        if (scene.name != "Lobby")
        {
            Cursor.visible = escCanvas.activeSelf;
            if (escCanvas.activeSelf)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnDisconnectBtn()
    {
        GameObject player;
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Lobby")
            player = GameObject.Find("LocalRoomPlayerObject");
        else
            player = GameObject.Find("LocalPlayerObject");
        player.GetComponent<NetworkIdentity>().connectionToServer.Disconnect();
        if (player.GetComponent<NetworkIdentity>().isServer)
            GameObject.Find("NetworkRoomManager").GetComponent<NetworkRoomManagerExt>().StopServer();
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            SceneManager.sceneLoaded -= OnSceneChanged; 
            Destroy(gameObject);
        }
    }
}
