using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        #region Property data
        [SerializeField] private Vector2 _healthRange; private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _attackDamageRange;  private float _attackDamage;
        [SerializeField] private Vector2 _attackDelaySecondsRange; private float _attackDelaySeconds;
        [SerializeField] private float _attackRange;
        [SerializeField] private Vector2 _compostRewardRange; private int _compostReward; 
        [SerializeField] bool _randomizeEachAttackDamage = true;
        [SerializeField] bool _randomizeEachAttackDelay = true;
        #endregion

        #region Effects tracking data
        private float _activeSlowPercentage = 0f;
        private float _remainingSlowDuration = 0f;
        private float _remainingStunDuration = 0f;
        private float _activeDamagePerSecond = 0f;
        private float _remainingDotDuration = 0f;
        #endregion

        #region Behaviour data
        public List<Vector3> MovementPoints => _movementPoints;
        private List<Vector3> _movementPoints = new List<Vector3>();
        private float _timeSinceLastAttack = 0f;
        private float _timeSinceLastDamageTick = 0f;

        Color _originalColor;
        Renderer _renderer;
        #endregion
        
        private void Start()
        {
            _health = GetRandomInRange(_healthRange);
            if(!_randomizeEachAttackDamage) _attackDamage = GetRandomInRange(_attackDamageRange);
            if(!_randomizeEachAttackDelay) _attackDelaySeconds = GetRandomInRange(_attackDelaySecondsRange);   
            _compostReward = Mathf.RoundToInt(GetRandomInRange(_compostRewardRange));

            _renderer = GetComponentInChildren<Renderer>();
            if (_renderer != null) _originalColor = _renderer.material.color;
        }

        public void Initialize(List<Vector3> path)
        {
            EnemyManager.Instance.RegisterEnemy();

            _movementPoints = path;

            POE target = EnemyManager.Instance.GetTarget();
            Vector2 randomInCircle = Random.insideUnitCircle;
            Vector3 attackOffset = new Vector3(randomInCircle.x, 0f, randomInCircle.y) * target.Collider.radius;
            _movementPoints.Add(target.transform.position + attackOffset);

            path.Reverse();
        }

        private void Update()
        {
            MoveTowardsTarget();
            UpdateEffects();
            HandleAttacking();
        }

        private void MoveTowardsTarget()
        {
            if (_remainingStunDuration > 0f) return;
            if(_movementPoints.Count == 0) return;

            float effectiveSpeed = _speed * (1f - _activeSlowPercentage / 100f);
            transform.position = Vector3.MoveTowards(transform.position, _movementPoints[_movementPoints.Count - 1], effectiveSpeed * Time.deltaTime);
            Vector3 lookDirection = _movementPoints[_movementPoints.Count - 1] - transform.position;
            if(lookDirection != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDirection);

            if(transform.position == _movementPoints[_movementPoints.Count - 1])
                _movementPoints.RemoveAt(_movementPoints.Count - 1);
        }

        private void UpdateEffects()
        {
            if (_activeDamagePerSecond > 0f)
            {
                _remainingDotDuration -= Time.deltaTime;
                _timeSinceLastDamageTick += Time.deltaTime;

                if (_timeSinceLastDamageTick >= 1f)
                {
                    TakeDamage(_activeDamagePerSecond);
                    _timeSinceLastDamageTick = 0f;
                }

                if (_remainingDotDuration <= 0f)
                    _activeDamagePerSecond = 0f;
            }

            if (_remainingSlowDuration > 0f)
            {
                _remainingSlowDuration -= Time.deltaTime;
                if (_remainingSlowDuration <= 0f)
                    _activeSlowPercentage = 0f;
            }

            if (_remainingStunDuration > 0f)
            {
                _remainingStunDuration -= Time.deltaTime;
                if (_remainingStunDuration <= 0f)
                    _remainingStunDuration = 0f;
            }
        }

        private void HandleAttacking()
        {
            if (_movementPoints.Count > 1) return; //Last movement point is the target position    
            if (_remainingStunDuration > 0f) return;
            if (Vector3.Distance(transform.position, EnemyManager.Instance.GetTarget().transform.position) > _attackRange) return;

            _timeSinceLastAttack += Time.deltaTime;

            if (_timeSinceLastAttack >= _attackDelaySeconds)
            {
                float damage = _randomizeEachAttackDamage ? GetRandomInRange(_attackDamageRange) : _attackDamage;
                EnemyManager.Instance.GetTarget().TakeDamage(damage);

                _timeSinceLastAttack = 0f;

                if(_randomizeEachAttackDelay)
                    _attackDelaySeconds = GetRandomInRange(_attackDelaySecondsRange);
            }
        }

        public void ApplyDot(float damagePerSecond, float duration)
        {
            _activeDamagePerSecond = damagePerSecond;
            _remainingDotDuration = duration;
        }

        public void ApplySlow(float slowPercentage, float duration)
        {
            _activeSlowPercentage = slowPercentage;
            _remainingSlowDuration = duration;
        }

        public void ApplyStun(float duration)
        {
            _remainingStunDuration = duration;
        }

        public void TakeDamage(float damageAmount)
        {
            _health -= damageAmount;
            if (_health <= 0f)
            {
                EnemyManager.Instance.UnregisterEnemy();
                Destroy(gameObject);
                return;
            }

            StartCoroutine(FlashRedCoroutine());
        }

        private IEnumerator FlashRedCoroutine()
        {
            if (_renderer == null)yield break;

            _renderer.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            if (_renderer == null) yield break;

            _renderer.material.color = _originalColor;
        }

        private float GetRandomInRange(Vector2 range) => Random.Range(range.x, range.y);

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer != GameConstants.Layers.POE)
                return;

            _speed = 0f;
            _attackRange = 666f; //Just a big number
        }
    }
}