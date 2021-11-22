using UnityEngine;
using Game;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Transform _rootPlayerSpawnPoint;
    private Transform[] _spawnPoints;
    
    enum Mode { Easy, Medium, Hard }
    [Space, SerializeField] private Mode _mode;

    public List<PlayerController> Players { get; private set; }

    private void Awake()
    {
        _spawnPoints = new Transform[_rootPlayerSpawnPoint.childCount];
        for (int i = 0; i < _spawnPoints.Length; i++)
            _spawnPoints[i] = _rootPlayerSpawnPoint.GetChild(i);

        Players = new List<PlayerController>();
        foreach (var spawnPoint in _spawnPoints)
        {
            Players.Add(CreatePlayer(spawnPoint.position));
        }
    }

    public PlayerController CreatePlayer(Vector3 position)
    {
        PlayerController playerController = Instantiate(_playerPrefab, position, Quaternion.identity);

        return playerController;
    }

    public string GetMode()
    {
        string mode = null;
        
        switch (_mode)
        {
            case Mode.Easy:
                mode = "Easy";
                break;
            case Mode.Medium:
                mode = "Medium";
                break;
            case Mode.Hard:
                mode = "Hard";
                break;
            default:
                Debug.LogError("Mod is not assigned");
                break;
        }

        return mode;
    }
}
