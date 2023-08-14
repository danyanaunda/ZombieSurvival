using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingGame : MonoBehaviour
{
    private const int INDEX_OF_BATTLESCENE = 1;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Play()
    {
        SceneManager.LoadScene(INDEX_OF_BATTLESCENE);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}