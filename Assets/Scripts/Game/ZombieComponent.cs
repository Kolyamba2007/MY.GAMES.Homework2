using System.Collections;
using UnityEngine;

namespace Game
{
    public partial class ZombieComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _aliveView;

        [SerializeField] private GameObject _diedView;

        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Vector3[] _deltaPath;

        private ZombieConfiguration _param;

        private GameManager _gameManager;
        private int _currentPoint = 0;
        private Vector3 _initPosition;

        private ZombieState _state = ZombieState.Wander;

        public ZombieState State => _state;

        private void Awake()
        {
            _initPosition = transform.position;
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            SetState(true);

            if(_gameManager != null)
                _param = (ZombieConfiguration)Resources.Load($"Configurations/{_gameManager.GetMode()}ZombieConfiguration");
            else
                _param = (ZombieConfiguration)Resources.Load($"Configurations/EasyZombieConfiguration");

            StartCoroutine(Wander());
        }

        private IEnumerator Wander()
        {
            while(_state == ZombieState.Wander && IsAlive)
            {
                if (!(_deltaPath == null || _deltaPath.Length < 2))
                {
                    var direction = _initPosition + _deltaPath[_currentPoint] - transform.position;
                    _rigidbody.velocity = direction.normalized * _param.Speed;

                    if (direction.magnitude <= 0.1f)
                    {
                        _currentPoint = (_currentPoint + 1) % _deltaPath.Length;
                    }
                }

                yield return null;
            }

            yield break;
        }

        private IEnumerator Attack(PlayerController target)
        {
            Transform player = target.gameObject.transform;

            while (_state == ZombieState.Attack && IsAlive && target != null)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance > _param.AttackDistance)
                {
                    var direction = player.position - transform.position;
                    _rigidbody.velocity = direction.normalized * 100 * _param.Speed * Time.deltaTime;
                }
                else
                {
                    target.Hitpoints -= _param.Damage;
                    yield return new WaitForSeconds(1);
                }

                yield return null;
            }

            _state = ZombieState.Wander;
            StartCoroutine(Wander());
            yield break;
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.layer == 15 && _state != ZombieState.Attack)
            {
                _state = ZombieState.Attack;
                StartCoroutine(Attack(collider.GetComponentInParent<PlayerController>()));
            }
        }

        public void SetState(bool alive)
        {
            _aliveView.SetActive(alive);
            _diedView.SetActive(!alive);
        }

        public bool IsAlive => _aliveView.activeInHierarchy;
    }
}