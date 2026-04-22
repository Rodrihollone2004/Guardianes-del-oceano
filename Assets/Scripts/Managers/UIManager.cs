using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text contaminationText;
    [SerializeField] private TMP_Text alertText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;

    public void UpdateContamination(float percentage)
    {
        contaminationText.text = $"Contamination: {Mathf.RoundToInt(percentage)}%";
    }

    public void ShowWinScreen() => winPanel.SetActive(true);
    public void ShowLoseScreen() => losePanel.SetActive(true);
    public void ShowPauseScreen() => pausePanel.SetActive(true);
    public void ResumeGame() => pausePanel.SetActive(false);

    public void ShowAlert(string message, Color color)
    {
        alertText.text = message;
        alertText.color = color;
        alertText.gameObject.SetActive(true);

        Invoke("HideAlert", 1.0f);
    }

    private void HideAlert() => alertText.gameObject.SetActive(false);
}

