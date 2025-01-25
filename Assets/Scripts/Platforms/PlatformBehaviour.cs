using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool _explode;
    [SerializeField]
    private bool _is_temporary;
    [SerializeField]
    private Transform _player_1_transform;
    [SerializeField]
    private Transform _player_2_transform;
    [SerializeField]
    private BoxCollider2D _box_1_collider;
    [SerializeField]
    private BoxCollider2D _box_2_collider;
    [SerializeField]
    private CircleCollider2D _circle_collider;
    private Rigidbody2D _rigidbody2D;
    private Transform _platform_deadline;
    private SpriteRenderer _sprite_renderer;
    [SerializeField]
    private ParticleSystem _particle_system;
    [SerializeField]
    [Range(0f, 20f)]
    public float _speed = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    public float _weight_decrement;
    [SerializeField]
    public int _score;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _player_1_transform = GameObject.FindGameObjectWithTag("Player1").transform;
        _player_2_transform = GameObject.FindGameObjectWithTag("Player2").transform;
        _platform_deadline = GameObject.FindGameObjectWithTag("Platform_Deadline").transform;
        _box_1_collider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        _box_2_collider = transform.GetChild(1).GetComponent<BoxCollider2D>();
        _circle_collider = GetComponent<CircleCollider2D>();
        _sprite_renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(_platform_deadline.position.y < transform.position.y)
        {
            Disable();
        }

        if(_explode)
        {
            return;
        }

        if(_player_1_transform.position.y < transform.position.y)
        {
            _box_1_collider.enabled = false;
        }
        else
        {
            _box_1_collider.enabled = true;
        }

        if (_player_2_transform.position.y < transform.position.y)
        {
            _box_2_collider.enabled = false;
        }
        else
        {
            _box_2_collider.enabled = true;
        }
    }

    private void Disable()
    {
        if (_is_temporary)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(0f, _speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER1) || collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER2))
        {
            _speed -= _weight_decrement;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER1) || collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER2))
        {
            _speed += _weight_decrement;
        }
    }

    public void SetIsTemporary(bool value)
    {
        _is_temporary = value;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetExplode(bool value)
    {
        _explode = value;
    }

    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    public int GetScore()
    {
        return _score;
    }

    public void Explode()
    {
        _explode = true;
        _box_1_collider.enabled = false;
        _box_2_collider.enabled = false;
        _circle_collider.enabled = false;
        _speed = 0f;
        _particle_system.Play();
        _sprite_renderer.enabled = false;
        Invoke("Disable", 2f);
    }
}
