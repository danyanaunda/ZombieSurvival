using System;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private EndGame _endGame;
    private bool isEnabledMenu = true;

    
    private void Awake()
    {
        _endGame = GetComponent<EndGame>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            SetPause(isEnabledMenu);
        }
    }
    
    
    private void SetPause(bool status)
    {
        _endGame.OpenMenu(status);
        Cursor.lockState = status ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = status;
        Time.timeScale = status ? 0 : 1f;
        isEnabledMenu = !isEnabledMenu;

    }
}