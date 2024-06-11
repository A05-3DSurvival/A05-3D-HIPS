using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void StartGame()
    {
        LoadSceneManger.Instance.LoadScene("MainScene");
    }
    public void Setting()
    {
        UIManager.Instance.ShowSettingPanel();
    }
    public void Close()
    {
        UIManager.Instance.HideSettingPanel();
    }
    public void MainMenu()
    {
        LoadSceneManger.Instance.LoadScene("StartScenes");
    }
}
