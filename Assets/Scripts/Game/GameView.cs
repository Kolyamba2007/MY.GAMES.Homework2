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

            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].Hitpoints > 0)
                    _players[i].Hitpoints -= Time.deltaTime;
                else
                {
                    Destroy(_players[i].gameObject);
                    _players.Remove(_players[i]);
                }
            }

            if(_players.Count == 0)
            {
                _gameOverBlock.SetActive(true);
                return;
            }
        }
    }
}