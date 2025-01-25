using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
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
    private Rigidbody2D _rigidbody2D;
    private Transform _platform_deadline;
    [SerializeField]
    [Range(0f, 20f)]
    public float _speed = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    public float _weight_decrement;

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
    }

    private void Update()
    {
        if(_platform_deadline.position.y < transform.position.y)
        {
            if(_is_temporary)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
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

    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
