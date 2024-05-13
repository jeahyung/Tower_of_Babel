using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : Singleton<Test>
{
    public static int score;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(score);
    }

    public void SetScore(int i)
    {
        score += i;
    }

    public void LoadSceen()
    {
        SceneManager.LoadScene("Test 1");
    }
}
