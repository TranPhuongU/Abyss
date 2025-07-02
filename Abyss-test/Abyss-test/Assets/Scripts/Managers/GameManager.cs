using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Transform player;
    [SerializeField] UI_FadeScreen fadeScreen;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    public bool isLoadingGame = false;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        player = PlayerManager.instance.player.transform;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    private void LoadCheckpoints(GameData _data)
    {
        string currentMap = SceneManager.GetActiveScene().name;
        if (!_data.allMapCheckpoints.ContainsKey(currentMap)) return;

        var mapCheckpoints = _data.allMapCheckpoints[currentMap];

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (mapCheckpoints.ContainsKey(checkpoint.id) && mapCheckpoints[checkpoint.id])
                checkpoint.ActivateCheckpoint();
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        isLoadingGame = true;

        yield return new WaitForSeconds(.1f);

        LoadCheckpoints(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);

        isLoadingGame = false;
    }

    public void SaveData(ref GameData _data)
    {
        string currentMap = SceneManager.GetActiveScene().name;

        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        // Lưu checkpoint theo map
        if (!_data.allMapCheckpoints.ContainsKey(currentMap))
            _data.allMapCheckpoints[currentMap] = new SerializableDictionary<string, bool>();
        else
            _data.allMapCheckpoints[currentMap].Clear();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.allMapCheckpoints[currentMap][checkpoint.id] = checkpoint.activationStatus;
        }

        // Lưu checkpoint gần nhất
        if (!_data.mapClosestCheckpointIds.ContainsKey(currentMap))
            _data.mapClosestCheckpointIds.Add(currentMap, closestCheckpointId);
        else
            _data.mapClosestCheckpointIds[currentMap] = closestCheckpointId;
    }

    private void LoadClosestCheckpoint(GameData _data)
    {
        string currentMap = SceneManager.GetActiveScene().name;

        if (_data.mapClosestCheckpointIds.ContainsKey(currentMap))
            closestCheckpointId = _data.mapClosestCheckpointIds[currentMap];
        else
            closestCheckpointId = string.Empty;

        Checkpoint fallback = null;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.id == closestCheckpointId)
            {
                player.position = checkpoint.transform.position;
                return;
            }

            if (checkpoint.isDefaultCheckpoint)
                fallback = checkpoint;
        }

        if (fallback != null)
        {
            Debug.Log("Không tìm thấy checkpoint đã lưu. Load về checkpoint mặc định.");
            player.position = fallback.transform.position;
        }
    }

    public void PauseGame(bool _pause)
    {
        Time.timeScale = _pause ? 0 : 1;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        player = PlayerManager.instance.player.transform;

        SaveManager.instance.LoadGame();
    }

    public void SetClosestCheckpoint(Checkpoint checkpoint)
    {
        closestCheckpointId = checkpoint.id;
    }

    public void UnlockMap(string mapName)
    {
        if (!SaveManager.instance.gameData.unlockedMaps.Contains(mapName))
        {
            SaveManager.instance.gameData.unlockedMaps.Add(mapName);
            SaveManager.instance.SaveGame();
            Debug.Log($"🔓 Đã mở khóa map: {mapName}");
        }
    }
}
