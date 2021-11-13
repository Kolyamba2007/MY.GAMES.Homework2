using UnityEngine;
using Game;
using System.Collections.Generic;

public class PlayerFactory : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    public List<PlayerController> Players { get; private set; }

    private void Start()
    {
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
}
