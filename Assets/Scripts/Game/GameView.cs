using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private PlayerFactory _playerFactory;
        [SerializeField] private ZombieMap _zombieMap;
        
        [SerializeField] private GameObject _winBlock;
        [SerializeField] private GameObject _gameOverBlock;

        private List<PlayerController> _players;

        private void Start()
        {
            _players = _playerFactory.Players;
        }

        private void Update()
        {
            if (!_zombieMap.AlivePositions().Any())
            {
                _winBlock.SetActive(true);
                return;
            }

            foreach(var player in _players)
            {
                if (player.Hitpoints > 0)
                    player.Hitpoints -= Time.deltaTime;
                else
                {
                    _players.Remove(player);
                    Destroy(player.gameObject);
                }
            }

            if(_players.Count == 0)
                _gameOverBlock.SetActive(true);
        }
    }
}