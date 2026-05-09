using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text contaminationText;
    [SerializeField] private TMP_Text alertText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject albumPanel;

    private void Start()
    {
        GameManager.Instance.UIManager = this;
        UpdateContamination(0);
    }

    public void UpdateContamination(float percentage)
    {
        contaminationText.text = $"CONTAMINATION: {Mathf.RoundToInt(percentage)}";
    }

    public void ShowWinScreen() => winPanel.SetActive(true);
    public void ShowLoseScreen() => losePanel.SetActive(true);
    public void ShowPauseScreen() => pausePanel.SetActive(true);
    public void ShowAlbumScreen() => albumPanel.SetActive(true);
    public void ResumeAlbum() => albumPanel.SetActive(false);
    public void ResumeGame() => pausePanel.SetActive(false);

    public void ShowAlert(string message, Color color)
    {
        alertText.text = message;
        alertText.color = color;
        alertText.gameObject.SetActive(true);

        Invoke("HideAlert", 1.0f);
    }

    private void HideAlert() => alertText.gameObject.SetActive(false);

    public void AlbumSet()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowAlbumScreen();
        Time.timeScale = 0f;
    }

    public void HideAlbum()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        ResumeAlbum();
        Time.timeScale = 1f;
    }

    public void HidePause()
    {
        GameManager.Instance.TogglePause();
    }

    public void RestartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        GameManager.Instance.IsGameOver = false;
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            SceneManager.LoadScene(nextSceneIndex);

            GameManager.Instance.RestartValues();
        }
    }

    public void QuitGame() => Application.Quit();
}

