using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers References")]

    [HideInInspector] public TrashSpawner TrashSpawner;
    [HideInInspector] public TrashTrigger TrashTrigger;
    [HideInInspector] public UIManager UIManager;

    [Header("Album Configuration")]
    [SerializeField] private List<AnimalsSO> allFishInGame;
    private int currentUnlockIndex = 0;

    [Header("Game Settings")]
    [SerializeField] private float loseThreshold = 60f;

    private bool hasUnseenFish = false;

    private int totalTrashSpawned;
    private int trashProcessed;
    private float currentContamination;

    private bool isGameOver;
    private bool isAlbum;
    private bool isPaused;

    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public bool IsAlbum { get => isAlbum; set => isAlbum = value; }
    public bool IsPaused { get => isPaused; set => isPaused = value; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }
    }


    private void Start()
    {
        TrashTrigger.OnContamination += HandleTrashToBottom;

        totalTrashSpawned = TrashSpawner.SpawnLimit;
        currentContamination = 0;
        trashProcessed = 0;
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (TrashTrigger != null) TrashTrigger.OnContamination -= HandleTrashToBottom;
    }
    private void Update()
    {
        if (InputManager.Instance.WasPausePressedThisFrame() && !isGameOver && !isAlbum)
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused) UIManager.ShowPauseScreen();
        else UIManager.ResumeGame();

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Confined;
    }

    public void AddRawContamination(float amount)
    {
        if (isGameOver) return;

        currentContamination += amount;
        UIManager.UpdateContamination(currentContamination);

        if (currentContamination >= loseThreshold)
            GameOver(false);
    }

    private void HandleTrashToBottom()
    {
        if (isGameOver) return;

        float valuePerTrash = 100f / totalTrashSpawned;
        AddRawContamination(valuePerTrash);

        trashProcessed++;
        CheckWinCondition();
    }

    public void NotifyWrongRecycle()
    {
        if (isGameOver) return;

        UIManager.ShowAlert("WRONG BIN!", Color.red);

        // Penalización: Suma contaminación como si hubiera caído al fondo
        float penalty = 100f / totalTrashSpawned;
        AddRawContamination(penalty);

        trashProcessed++;
        CheckWinCondition();
    }

    public void NotifyTrashRecycled()
    {
        if (isGameOver) return;

        trashProcessed++;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (trashProcessed >= totalTrashSpawned && currentContamination < loseThreshold)
        {
            GameOver(true);
        }
    }

    private void GameOver(bool win)
    {
        isGameOver = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (win)
        {
            UnlockNextFish();
            UIManager.ShowWinScreen();
        }
        else UIManager.ShowLoseScreen();
    }

    private void UnlockNextFish()
    {
        if (currentUnlockIndex < allFishInGame.Count)
        {
            allFishInGame[currentUnlockIndex].isUnlocked = true;
            hasUnseenFish = true;

            if (UIManager != null && UIManager.AlbumNotification != null)
                UIManager.AlbumNotification.NotifyNewFish();

            currentUnlockIndex++;
        }
    }

    public void ClearUnseenFish()
    {
        hasUnseenFish = false;
    }

    public bool HasUnseenFish() => hasUnseenFish;
    public List<AnimalsSO> GetAllFish() => allFishInGame;

    public void RestartValues()
    {
        isGameOver = false;

        totalTrashSpawned = TrashSpawner.SpawnLimit;
        currentContamination = 0;
        trashProcessed = 0;
    }

    public void ResetFullGameProgression()
    {
        currentUnlockIndex = 0;
        hasUnseenFish = false;
        isGameOver = false;

        if (allFishInGame != null)
            foreach (AnimalsSO fish in allFishInGame)
                if (fish != null)
                    fish.isUnlocked = false;

        currentContamination = 0;
        trashProcessed = 0;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        TrashSpawner = FindFirstObjectByType<TrashSpawner>();
        TrashTrigger = FindFirstObjectByType<TrashTrigger>();
        UIManager = FindFirstObjectByType<UIManager>();

        if (TrashTrigger != null)
            TrashTrigger.OnContamination += HandleTrashToBottom;

        if (TrashSpawner != null)
            RestartValues();
    }
}