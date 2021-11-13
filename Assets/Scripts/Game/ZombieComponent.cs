using System.Collections;
using UnityEngine;

namespace Game
{
    public class ZombieComponent : MonoBehaviour
    {
        [SerializeField] private float _attackDistance;

        [SerializeField] private float _damage;

        [SerializeField] private float _speed;

        [SerializeField] private GameObject _aliveView;

        [SerializeField] private GameObject _diedView;

        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Vector3[] _deltaPath;

        private int _currentPoint = 0;
        private Vector3 _initPosition;

        enum State { Wander, Attack }

        private State _state = State.Wander;

        private void Awake()
        {
            _initPosition = transform.position;
        }

        private void OnEnable()
        {
            SetState(true);
            StartCoroutine(Wander());
        }

        private IEnumerator Wander()
        {
            while(_state == State.Wander && IsAlive)
            {
                if (!(_deltaPath == null || _deltaPath.Length < 2))
                {
                    var direction = _initPosition + _deltaPath[_currentPoint] - transform.position;
                    _rigidbody.velocity = direction.normalized * _speed;

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

            while (_state == State.Attack && IsAlive && target != null)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance > _attackDistance)
                {
                    var direction = player.position - transform.position;
                    _rigidbody.velocity = direction.normalized * 100 * _speed * Time.deltaTime;
                }
                else
                {
                    target.Hitpoints -= _damage;
                    Debug.Log(target.Hitpoints);
                    yield return new WaitForSeconds(1);
                }

                yield return null;
            }

            _state = State.Wander;
            StartCoroutine(Wander());
            yield break;
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.layer == 15 && _state != State.Attack)
            {
                _state = State.Attack;
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