using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManger : MonoBehaviour
{
    private static LoadSceneManger _instance;

    public static LoadSceneManger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadSceneManger>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("LoadSceneManger");
                    _instance = singletonObject.AddComponent<LoadSceneManger>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
