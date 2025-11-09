using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(SceneName sceneName)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene((int)sceneName);
    }

    public enum SceneName
    {
        Map = 0,
        Game
    }

}
