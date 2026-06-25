using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Texts")]
    [SerializeField] private TMP_Text contaminationText;
    [SerializeField] private TMP_Text alertText;

    [Header("UI Animated Panels")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private UIPanel winPanel;
    [SerializeField] private UIPanel losePanel;
    [SerializeField] private UIPanel pausePanel;
    [SerializeField] private UIPanel albumPanel;
    [SerializeField] private UIPanel mainMenuPanel;
    [SerializeField] private UIPanel optionsPanel;
    [SerializeField] private UIPanel tutorialPanel;

    [Header("Feedback UI")]
    [SerializeField] private AlbumNotification albumNotification;
    [SerializeField] private AudioClip clicButton;

    private UIPanel actualPanel;

    public AlbumNotification AlbumNotification { get => albumNotification; set => albumNotification = value; }

    private void Start()
    {
        StartGame();

        GameManager.Instance.UIManager = this;
        UpdateContamination(0);

        if (GameManager.Instance.HasUnseenFish() && albumNotification != null)
            albumNotification.NotifyNewFish();
    }

    private void StartGame()
    {
        if (mainMenuPanel != null)
        {
            actualPanel = mainMenuPanel;
            mainMenuPanel.Show();
        }

        fadePanel.alpha = 1f;

        GameManager.Instance.SetTransitioning(true);
        ShowTutorialScreen();
        fadePanel.DOFade(0f, 1f).SetUpdate(true).OnComplete(() =>
        {
            GameManager.Instance.SetTransitioning(false);
        });
    }

    public void UpdateContamination(float percentage)
    {
        if (contaminationText != null)
            contaminationText.text = $"CONTAMINATION: {Mathf.RoundToInt(percentage)}";
    }

    private void OpenPanel(UIPanel newPanel)
    {
        if (actualPanel == newPanel) return;

        if (actualPanel != null)
            actualPanel.Hide();

        AudioManager.Instance.PlaySFX2D(clicButton);
        newPanel.Show();
        actualPanel = newPanel;
    }

    public void CloseCurrentPanel()
    {
        if (actualPanel != null)
        {
            AudioManager.Instance.PlaySFX2D(clicButton);
            actualPanel.Hide();
            actualPanel = null;
        }
    }

    public void ShowMenuScreen() => OpenPanel(mainMenuPanel);
    public void ShowWinScreen() => OpenPanel(winPanel);
    public void ShowLoseScreen() => OpenPanel(losePanel);
    public void ShowPauseScreen() => OpenPanel(pausePanel);
    public void ShowOptionsScreen() => OpenPanel(optionsPanel);
    public void ShowAlbumScreen() { if (!GameManager.Instance.IsTutorialOpen) albumPanel.Show(); }

    public void ResumeAlbum() => albumPanel.Hide();
    public void ResumeGame() => CloseCurrentPanel();
    public void BackOptions(UIPanel panelUI) => OpenPanel(panelUI);

    public void ShowTutorialScreen()
    {
        if (tutorialPanel != null)
        {
            GameManager.Instance.IsTutorialOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            OpenPanel(tutorialPanel);
        }
    }

    public void CloseTutorial()
    {
        tutorialPanel.Hide();
        GameManager.Instance.IsTutorialOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1f;
    }

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
        AudioManager.Instance.PlaySFX2D(clicButton);
        GameManager.Instance.IsAlbum = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (albumNotification != null)
            albumNotification.ClearNotification();

        GameManager.Instance.ClearUnseenFish();

        ShowAlbumScreen();
        Time.timeScale = 0f;
    }

    public void HideAlbum()
    {
        AudioManager.Instance.PlaySFX2D(clicButton);
        GameManager.Instance.IsAlbum = false;
        ResumeAlbum();

        if (!GameManager.Instance.IsGameOver && !GameManager.Instance.IsPaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1f;
        }
    }

    public void HidePause() => GameManager.Instance.TogglePause();

    public void RestartGame()
    {
        AudioManager.Instance.PlaySFX2D(clicButton);
        GameManager.Instance.SetTransitioning(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (albumNotification != null)
            albumNotification.ClearNotification();

        GameManager.Instance.ResetFullGameProgression();

        Time.timeScale = 1f;

        Sequence playSequence = DOTween.Sequence().SetUpdate(true);
        playSequence.Append(fadePanel.DOFade(1f, 1f));

        playSequence.OnComplete(() =>
        {
            DOTween.KillAll();
            SceneManager.LoadScene(1);
        });
    }

    public void NextLevel()
    {
        AudioManager.Instance.PlaySFX2D(clicButton);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            GameManager.Instance.SetTransitioning(true);

            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            Sequence playSequence = DOTween.Sequence().SetUpdate(true);
            playSequence.Append(fadePanel.DOFade(1f, 1f));

            playSequence.OnComplete(() =>
            {
                DOTween.KillAll();
                SceneManager.LoadScene(nextSceneIndex);
                GameManager.Instance.RestartValues();
            });
        }
    }

    public void BackToMenu()
    {
        AudioManager.Instance.PlaySFX2D(clicButton);
        GameManager.Instance.SetTransitioning(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Time.timeScale = 1f;

        Sequence playSequence = DOTween.Sequence().SetUpdate(true);
        playSequence.Append(fadePanel.DOFade(1f, 1f));

        playSequence.OnComplete(() =>
        {
            DOTween.KillAll();
            SceneManager.LoadScene(0);
        });
    }

    public void QuitGame()
    {
        Application.Quit();
        AudioManager.Instance.PlaySFX2D(clicButton);
    }
}

