
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    #region FIELDS
    [SerializeField] private float _searchDistance;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private float _timeAttackDelay;
    [SerializeField] private GameObject _player;
    
    private Animator _animator;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private Transform _startPointForwardRay;
    private Transform _startPointBackRay;

    private bool isChasing;
    private bool _isGround;
    private float _health;
    private float _time;
    #endregion

    void Start()
    {
        _time = 0;
        _health = 100;
        _isGround = true;

        #region SETUPFIELD
        _animator = gameObject.GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _startPointForwardRay = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        _startPointBackRay = this.gameObject.transform.GetChild(1).GetComponent<Transform>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<Collider2D>();
        #endregion
    }

    void Update()
    {
        _time += Time.deltaTime;

        if(_health <= 0)
        {
            _animator.SetTrigger("Destroy");
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _collider.enabled = false;
            Invoke("Destroy", 0.5f);
        }

        if (SeePlayer())
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
            _animator.SetFloat("speed", 0);
        }
    }

    void FixedUpdate()
    { 
        if (isChasing)
        {
            ChasePlayer();
        }
    }

    private bool SeePlayer()
    {
        Vector2 directionToPlayer = (_player.transform.position - transform.position).normalized;

        RaycastHit2D hit1 = Physics2D.Raycast(_startPointForwardRay.position, directionToPlayer, _searchDistance);
        RaycastHit2D hit2 = Physics2D.Raycast(_startPointBackRay.position, directionToPlayer, _searchDistance / 2);

        
        if (hit1.collider != null && hit1.collider.gameObject.tag == "Player")
        {  
            return true;
        }

        if (hit2.collider != null && hit2.collider.gameObject.tag == "Player")
        {
            return true; 
        }

        return false;
    
    }

    private void ChasePlayer()
    {
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        direction.y = 0;

        if (_player.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //_rb.velocity = direction * _moveSpeed * Time.deltaTime;
        if (_isGround && Vector3.Distance(_player.transform.position, transform.position) >   attackDistance)
        {
                _rb.MovePosition(transform.position + direction * _moveSpeed * Time.fixedDeltaTime);
        }


        if (_isGround)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) <= attackDistance)
            {
                _animator.SetBool("attack", true);
                _animator.SetFloat("speed", 0);

                if (_time >= _timeAttackDelay)
                {
                    _time = 0;
                    _player.GetComponent<PlayerController>().HP -= Random.Range(5, 10); 
                }
            }
            else
            {
                _animator.SetBool("attack", false);
                _animator.SetFloat("speed", 1);
            }
        }
        else
        {
            _animator.SetFloat("speed", 0);
        }
    }

    private void OnDrawGizmos()
    {
        if (_player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (_player.transform.position - transform.position).normalized * _searchDistance);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            _isGround = true;
        }
        else if (collision.gameObject.tag == "gift")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            _isGround = false;
        }
    }

    public void GetDamage(float damage)
    {
        _health -= damage;
        StartCoroutine(DamageColor());
    }

    IEnumerator DamageColor()
    {
        for (int i = 0; i <= 10; i++)
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.01f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
