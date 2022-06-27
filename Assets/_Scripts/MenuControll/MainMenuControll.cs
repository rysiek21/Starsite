using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MainMenuControll : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject modeMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] TMP_InputField serverIP;
    [SerializeField] NetworkRoomManagerExt netManager;

    string currentMenu;

    private void Start()
    {
        SetActiveCanvas(mainMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMenu == "ModeSelect")
            {
                SetActiveCanvas(mainMenu);
            }
            else if (currentMenu == "OptionsMenu")
            {
                SetActiveCanvas(mainMenu);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToModeSelect()
    {
        SetActiveCanvas(modeMenu);
    }
    
    public void ToMainMenu()
    {
        SetActiveCanvas(mainMenu);
    }

    public void HostLocalGame()
    {
        netManager.StartHost();
    }
    public void ConnectToGame()
    {
        netManager.networkAddress = serverIP.text == "" ?  "localhost" : serverIP.text;
        netManager.StartClient();
    }

    private void SetActiveCanvas(GameObject active)
    {
        mainMenu.SetActive(false);
        modeMenu.SetActive(false);
        optionsMenu.SetActive(false);

        active.SetActive(true);
        currentMenu = active.name;
    }
}
