using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers References")]
    [SerializeField] TrashTrigger trashTrigger;
    [SerializeField] TrashSpawner trashSpawner;
    [SerializeField] UIManager uIManager;

    [Header("Album Configuration")]
    [SerializeField] private List<FishSO> allFishInGame;
    private int currentUnlockIndex = 0;

    [Header("Game Settings")]
    [SerializeField] private float loseThreshold = 60f;

    private int totalTrashSpawned;
    private int trashOnBottom;
    private int trashProcessed;

    private bool isGameOver;
    private bool isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        trashTrigger.OnContamination += HandleContamination;
    }

    private void OnDisable()
    {
        trashTrigger.OnContamination -= HandleContamination;
    }

    private void Start()
    {
        totalTrashSpawned = trashSpawner.SpawnLimit;
        uIManager.UpdateContamination(0);
    }

    private void Update()
    {
        if (InputManager.Instance.WasPausePressedThisFrame() && !isGameOver)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            uIManager.ShowPauseScreen();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            uIManager.ResumeGame();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void ProcessContamination()
    {
        if (isGameOver) return;

        trashOnBottom++;
        trashProcessed++;

        float currentPercentage = ((float)trashOnBottom / totalTrashSpawned) * 100f;
        uIManager.UpdateContamination(currentPercentage);

        if (currentPercentage >= loseThreshold)
        {
            GameOver(false);
        }
        else if (trashProcessed >= totalTrashSpawned)
        {
            GameOver(true);
        }
    }

    private void HandleContamination()
    {
        ProcessContamination();
    }

    public void NotifyWrongRecycle()
    {
        uIManager.ShowAlert("WRONG BIN!", Color.red);

        ProcessContamination();
    }

    public void NotifyTrashRecycled()
    {
        if (isGameOver) return;

        trashProcessed++;

        if (trashProcessed >= totalTrashSpawned)
        {
            GameOver(true);
        }
    }

    private void GameOver(bool win)
    {
        isGameOver = true;
        //Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (win)
        {
            UnlockNextFish(); 
            uIManager.ShowWinScreen();
        }
        else uIManager.ShowLoseScreen();
    }

    private void UnlockNextFish()
    {
        if (currentUnlockIndex < allFishInGame.Count)
        {
            allFishInGame[currentUnlockIndex].isUnlocked = true;
            Debug.Log($"Desbloqueado: {allFishInGame[currentUnlockIndex].fishName}");

            currentUnlockIndex++;
        }
    }

    public List<FishSO> GetAllFish() => allFishInGame;

    public void RestartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void QuitGame() => Application.Quit();
}