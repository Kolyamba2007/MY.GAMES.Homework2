using System.Linq;
using Game;
using UnityEngine;

namespace Search
{
    public class AStarBot : PlayerInput
    {
        private ZombieMap _zombieMap;
        private LevelMap _levelMap;
        [SerializeField] private Transform _player;
        [SerializeField] private float _fireDistance;


        private int[,] _map;
        private int _deltaX;
        private int _deltaZ;

        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            var alivePositions = _zombieMap.AlivePositions();
            if (alivePositions.Count == 0)
            {
                return (Vector3.zero, Quaternion.identity, false);
            }

            var playerPosition = _player.position;
            var targetPosition = ClosestEnemy(playerPosition, alivePositions);
            var from = ToInt(playerPosition);
            var to = ToInt(targetPosition);

            var path = AStarFromGoogle.FindPath(_map, @from, to);
            var nextPathPoint = path.Count >= 2 ? path[1] : to;
            nextPathPoint = new Vector2Int(nextPathPoint.x - _deltaX, nextPathPoint.y - _deltaZ);

            var moveDirection = new Vector3(nextPathPoint.x, playerPosition.y, nextPathPoint.y) - playerPosition;
            var directVector = targetPosition - playerPosition;

            return (moveDirection, Quaternion.LookRotation(directVector), directVector.magnitude <= _fireDistance);
        }

        private void Awake()
        {
            _zombieMap = FindObjectOfType<ZombieMap>();
            _levelMap = FindObjectOfType<LevelMap>();

            var maxX = _levelMap.Points.Max(p => Mathf.RoundToInt(p.x));
            var minX = _levelMap.Points.Min(p => Mathf.RoundToInt(p.x));

            var maxZ = _levelMap.Points.Max(p => Mathf.RoundToInt(p.z));
            var minZ = _levelMap.Points.Min(p => Mathf.RoundToInt(p.z));

            _deltaX = minX < 0 ? -minX : 0;
            _deltaZ = minZ < 0 ? -minZ : 0;

            _map = new int[maxX + _deltaX + 1, maxZ + _deltaZ + 1];

            foreach (var point in _levelMap.Points)
            {
                _map[_deltaX + Mathf.RoundToInt(point.x), _deltaZ + Mathf.RoundToInt(point.z)] = -1;
            }

        }

        private Vector3 ClosestEnemy(Vector3 playerPosition, System.Collections.Generic.List<Vector3> alivePositions)
        {
            if (alivePositions.Count() == 0) Debug.LogError("The collection contains no objects");

            Vector3 closestEnemyPosition = Vector3.zero;
            float minDistance = float.MaxValue;

            foreach (var enemy in alivePositions)
            {
                float distance = Vector3.Distance(playerPosition, enemy);

                if (distance < minDistance)
                {
                    closestEnemyPosition = enemy;
                    minDistance = distance;
                }
            }

            return closestEnemyPosition;
        }

        private Vector2Int ToInt(Vector3 vector3) =>
            new Vector2Int(_deltaX + Mathf.RoundToInt(vector3.x), _deltaZ + Mathf.RoundToInt(vector3.z));
    }
}