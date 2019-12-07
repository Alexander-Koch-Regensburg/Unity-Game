using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OptionsMenuCheckboxHandler : MonoBehaviour
{
    public Button FovCheckbox;
    public Button SocketCheckbox;

    private bool enableFov;
    private bool enableSocket;


    void Start()
    {
        enableFov = GetPlayerPref("EnemyFOV");
        enableSocket = GetPlayerPref("Socket");
        MainMenuPlayerPreferences.EnableFOV = enableFov;
        MainMenuPlayerPreferences.OpenSocket = enableSocket;
        ShowCheckbox(FovCheckbox, enableFov);
        ShowCheckbox(SocketCheckbox, enableSocket);
    }
    

    public void ToggleEnemeyFOV()
    {
        enableFov = !enableFov;
        SetPlayerPref("EnemyFOV", enableFov);
        ShowCheckbox(FovCheckbox, enableFov);
        MainMenuPlayerPreferences.EnableFOV = enableFov;
    }

    public void ToggleSocket()
    {
        enableSocket = !enableSocket;
        SetPlayerPref("Socket", enableSocket);
        ShowCheckbox(SocketCheckbox, enableSocket);
        MainMenuPlayerPreferences.OpenSocket = enableSocket;
    }

    private void ShowCheckbox(Button Checkbox, bool active) {
        GameObject text = Checkbox.transform.Find("Text").gameObject;
        text.SetActive(active);
    }

    private void SetPlayerPref(string key, bool active)
    {
        PlayerPrefs.SetInt(key, active ? 1 : 0);
    }

    private bool GetPlayerPref(string key)
    {
        return PlayerPrefs.GetInt(key) == 1 ? true : false;
    }
}
