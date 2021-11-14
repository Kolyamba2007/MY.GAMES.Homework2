using UnityEngine;

[CreateAssetMenu(fileName = "StandartZombieConfiguration", menuName = "Configurations/ZombieConfiguration", order = 3)]
public class ZombieConfiguration : ScriptableObject
{
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;

    /// <summary>
    /// Return attack distance
    /// </summary>
    public float AttackDistance => _attackDistance;
    /// <summary>
    /// Return damage
    /// </summary>
    public float Damage => _damage;
    /// <summary>
    /// Return speed
    /// </summary>
    public float Speed => _speed;
}
