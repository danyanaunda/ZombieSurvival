using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI textEndGame;

    private const int INDEX_OF_MAIN_MENU = 0;
    private const int INDEX_OF_BATTLESCENE = 1;
    
    public void Won()
    {
        textEndGame.text = $"Поздравляем, ТЫ ВЫЖИЛ!"; 
        ButtonStatus(true);
    }

    public void Lose()
    {
        textEndGame.text = $"Тебя съели, но ничего страшного, попробуй ещё раз!";
        retryButton.gameObject.SetActive(true);
        ButtonStatus(true);
    }

    public void OpenMenu(bool status)
    {
        ButtonStatus(status);
        textEndGame.gameObject.SetActive(false);
    }

    public void OnRetryClick()
    {
        ButtonStatus(false);
        SceneManager.LoadScene(INDEX_OF_BATTLESCENE);
    }
    
    public void OnMenuClick()
    {
        ButtonStatus(false);
        SceneManager.LoadScene(INDEX_OF_MAIN_MENU);
    }
    
    public void OnExitClick()
    {
        ButtonStatus(false);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ButtonStatus(bool status)
    {
        if (status == false) retryButton.gameObject.SetActive(false);
        Cursor.visible = status;
        Cursor.lockState = status? CursorLockMode.None : CursorLockMode.Locked;
        textEndGame.gameObject.SetActive(status);
        background.gameObject.SetActive(status);
        menuButton.gameObject.SetActive(status);
        exitButton.gameObject.SetActive(status);
    }
    
}
