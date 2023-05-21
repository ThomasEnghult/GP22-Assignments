using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;
using TMPro;

public class GameManager : AttributesSync
{
    private Alteruna.Avatar _avatar;

    public static GameManager Instance;
    private Spawner _spawner;
    private GameObject obstacleSpawner;

    public float gameOverDelay = 5f;

    public int alivePlayers = 0;

    private bool isStarter = false;
    public bool inGame = false;

    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private TextMeshProUGUI alivePlayersText;
    [SerializeField] private TextMeshProUGUI isAliveText;

    private List<GameObject> walls = new List<GameObject>();
    [SerializeField] private GameObject wallRight;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallUp;
    [SerializeField] private GameObject wallDown;

    [SerializeField] private StartLevelZone startLevelZone;

    [SerializeField] private List<Transform> SpawnLocations;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ClearWinnerText();
        _spawner = GetComponent<Spawner>();

        walls.Add(wallRight);
        walls.Add(wallLeft);
        walls.Add(wallUp);
        walls.Add(wallDown);

        SetupMultiplayer();
    }

    void Update()
    {
        alivePlayersText.text = "Alive Players: " + alivePlayers;
    }

    public void PlayerStarted()
    {
        isStarter = true;
        Multiplayer.LockRoom();
        InvokeRemoteMethod(nameof(OnStart), UserId.AllInclusive, UnityEngine.Random.state);
    }

    [SynchronizableMethod]
    public void OnStart(UnityEngine.Random.State state)
    {
        inGame = true;
        SetAliveText("Alive");
        UnityEngine.Random.state = state;
        alivePlayers = Multiplayer.CurrentRoom.GetUserCount();
        StartRandomGameMode();
    }

    void StartRandomGameMode()
    {
        int randomMode = UnityEngine.Random.Range(0, _spawner.SpawnableObjects.Count);
        Debug.Log(_spawner.SpawnableObjects.Count);

        switch (randomMode)
        {
            case 0:
                MovingWalls();
                break;
            case 1:
                FallingPlatforms();
                break;
        }
    }

    void MovingWalls()
    {
        wallUp.SetActive(false);
        wallDown.SetActive(false);
        Spawn(0, new Vector3(0, 0, 70));
    }

    void FallingPlatforms()
    {
        foreach(GameObject wall in walls)
        {
            wall.SetActive(false);
        }
        Spawn(1, Vector3.zero);
    }

    void Spawn(int index, Vector3 position)
    {
        if(isStarter)
            obstacleSpawner = _spawner.Spawn(index, position);
    }

    public void PlayerWon()
    {
        if (!inGame)
            return;

        InvokeRemoteMethod(nameof(SetWinnerText), UserId.AllInclusive, "Winner: " + Multiplayer.Me.Name + "!");
        Invoke(nameof(TriggerGameOver), gameOverDelay);
    }

    public void Draw()
    {
        InvokeRemoteMethod(nameof(SetWinnerText), UserId.AllInclusive, "Draw!");
        Invoke(nameof(TriggerGameOver), gameOverDelay);
    }

    void TriggerGameOver()
    {
        Multiplayer.UnlockRoom();
        InvokeRemoteMethod(nameof(OnGameOver), UserId.AllInclusive);
    }

    [SynchronizableMethod]
    void OnGameOver()
    {
        inGame = false;
        isStarter = false;

        _avatar.GetComponent<PlayerHealth>().Respawn();
        ClearWinnerText();
        _spawner.Despawn(obstacleSpawner);

        foreach(GameObject wall in walls)
        {
            wall.SetActive(true);
        }

        startLevelZone.SetEnabled(true);
    }

    public void SetAliveText(string text)
    {
        isAliveText.text = text;
    }

    public void PlayerDied()
    {
        alivePlayers--;
        alivePlayersText.text = ("Alive Players: " + alivePlayers);
    }

    [SynchronizableMethod]
    public void SetWinnerText(string text)
    {
        winnerText.text = text;
    }
    public void ClearWinnerText()
    {
        winnerText.text = "";
    }
    public void SetMyAvatar(Alteruna.Avatar avatar)
    {
        _avatar = avatar;
    }

    void SetupMultiplayer()
    {
        Multiplayer.AvatarSpawnLocations = SpawnLocations;
        Multiplayer.SpawnAvatar();

    }

}
