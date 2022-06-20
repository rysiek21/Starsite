using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ConsoleManager : MonoBehaviour
{
    [SerializeField] GameObject console;
    [SerializeField] GameObject commandInput;
    [SerializeField] GameObject commandOutput;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        console.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            console.SetActive(!console.activeSelf);
        }

        if (console.activeSelf)
            if (Input.GetKeyDown(KeyCode.Return))
                RunCommand();
    }

    public void RunCommand()
    {
        CommandController(commandInput.GetComponent<TMP_InputField>().text);
        commandInput.GetComponent<TMP_InputField>().text = "";
    }

    void CommandController(string command)
    {
        switch (command)
        {
            case "help":
                commandOutput.GetComponent<TMP_InputField>().text += "<color=green>Avaliable commands: help, heal \n";
                break;
            case "heal":
                
                break;
            default:
                commandOutput.GetComponent<TMP_InputField>().text += "<color=red>Unknown command. Try to use 'help' to check available. \n";
                break;
        }
    }
}
