using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool _enabled;
    [SerializeField]
    private int _player_id;
    [SerializeField]
    [Range(0f, 20f)]
    private float _speed = 1f;
    [SerializeField]

    //Max
    private float _dash_max_speed;
    private float _knockback_max_speed;

    //Speed
    private float _throwable_stun_speed = 0f;
    private float _dash_speed = 0f;

    private Vector2 _initial_attack_vector;
    private Vector2 _attack_vector;

    [SerializeField]
    [Range(10f, 20f)]
    private float _horizontal_boundary;
    [SerializeField]
    private SpriteRenderer _sprite_renderer;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _input_direction;
    private string _horizontal_axis;
    private string _vertical_axis;

    private KeyCode _jump_key_controller;
    private KeyCode _dash_key_controller;
    private KeyCode _throwable_key_controller;
    private KeyCode _attack_key_controller;

    private KeyCode _jump_key_keyboard;
    private KeyCode _dash_key_keyboard;
    private KeyCode _throwable_key_keyboard;
    private KeyCode _attack_key_keyboard;
    private int _jump_count;

    private float _last_knockback_time;
    private float _last_throwable_time;
    private float _last_dash_time;
    private float _last_attack_time;


    private bool _knockback;
    private bool _attack;
    [SerializeField]
    private GameObject _sword;
    [SerializeField]
    private GameObject _throwable_prefab;
    [SerializeField]
    private Transform _left_throwable_anchor;
    [SerializeField]
    private Transform _right_throwable_anchor;
    private Animator _animator_controller;

    private void Awake()
    {
        _last_attack_time = Time.time - PlayerConstants.ATTACK_COOLDOWN;
        _last_knockback_time = Time.time;
        _last_dash_time = Time.time - PlayerConstants.DASH_COOLDOWN;
        _last_throwable_time = Time.time - PlayerConstants.THROWABLE_COOLDOWN;
        _animator_controller = GetComponent<Animator>();

        _sprite_renderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _horizontal_axis = "Horizontal" + _player_id;
        _vertical_axis = "Vertical" + _player_id;

        _jump_key_controller = _player_id == 1 ? KeyCode.Joystick1Button0 : KeyCode.Joystick2Button0;
        _dash_key_controller = _player_id == 1 ? KeyCode.Joystick1Button2 : KeyCode.Joystick2Button2;
        _throwable_key_controller = _player_id == 1 ? KeyCode.Joystick1Button3 : KeyCode.Joystick2Button3;
        _attack_key_controller = _player_id == 1 ? KeyCode.Joystick1Button1 : KeyCode.Joystick2Button1;

        _jump_key_keyboard = _player_id == 1 ? KeyCode.E : KeyCode.O;
        _dash_key_keyboard = _player_id == 1 ? KeyCode.LeftShift : KeyCode.RightShift;
        _throwable_key_keyboard = _player_id == 1 ? KeyCode.R : KeyCode.P;
        _attack_key_keyboard = _player_id == 1 ? KeyCode.Q : KeyCode.U;

        _right_throwable_anchor = transform.GetChild(0);
        _left_throwable_anchor = transform.GetChild(1);

        _jump_count = PlayerConstants.JUMP_COUNT;
    }

    private void Update()
    {
        // Wrapping
        if (transform.position.x < -_horizontal_boundary)
        {
            transform.position = new Vector3(_horizontal_boundary - 0.5f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > _horizontal_boundary)
        {
            transform.position = new Vector3(-_horizontal_boundary + 0.5f, transform.position.y, transform.position.z);
        }

        //Movement
        _input_direction = new Vector2(Input.GetAxis(_horizontal_axis), Input.GetAxis(_vertical_axis));
        _input_direction.Normalize();


        //Flip
        if (_input_direction.x > 0f)
        {
            _sprite_renderer.flipX = false;
        }
        else if (_input_direction.x < 0f)
        {
            _sprite_renderer.flipX = true;
        }

        _animator_controller.SetBool("moving", Mathf.Abs(_input_direction.x) > 0f);


        if (!_enabled)
        {
            return;
        }

        //Jump
        if (_jump_count > 0 && (Input.GetKeyDown(_jump_key_controller) || Input.GetKeyDown(_jump_key_keyboard)))
        {
            _jump_count--;
            _rigidbody2D.AddForce(Vector2.up * PlayerConstants.JUMP_IMPULSE, ForceMode2D.Impulse);
        }

        //Dash
        if (Time.time - _last_dash_time > PlayerConstants.DASH_COOLDOWN && (Input.GetKeyDown(_dash_key_controller) || Input.GetKeyDown(_dash_key_keyboard)))
        {
            _last_dash_time = Time.time;
            _animator_controller.SetBool("dash", true);

            if (_sprite_renderer.flipX)
            {
                _dash_max_speed = PlayerConstants.DASH_MAX_SPEED * -1f;
            }
            else
            {
                _dash_max_speed = PlayerConstants.DASH_MAX_SPEED;
            }
            _dash_speed = _dash_max_speed;
        }
        else
        {
            _dash_speed = Mathf.Lerp(_dash_max_speed, 0f, Time.time - _last_dash_time / PlayerConstants.DASH_LERP_TIME);
            if (_dash_speed == 0f)
            {
                _animator_controller.SetBool("dash", false);
            }
        }

        //Attack
        if (Time.time - _last_attack_time > PlayerConstants.ATTACK_COOLDOWN && (Input.GetKeyDown(_attack_key_controller) || Input.GetKeyDown(_attack_key_keyboard)))
        {
            _animator_controller.SetBool("dash", true);
            _initial_attack_vector = _input_direction;
            _rigidbody2D.AddForce(Vector2.up * _initial_attack_vector.y * PlayerConstants.ATTACK_IMPULSE, ForceMode2D.Impulse);
            _last_attack_time = Time.time;
            _attack = true;

            float angle = Mathf.Atan2(_input_direction.y, _input_direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _sword.transform.rotation = rotation;
            _sword.gameObject.SetActive(true);
        }

        //Throwable
        if (Time.time - _last_throwable_time > PlayerConstants.THROWABLE_COOLDOWN && (Input.GetKeyDown(_throwable_key_controller) || Input.GetKeyDown(_throwable_key_keyboard)))
        {
            _last_throwable_time = Time.time;
            GameObject throwable;
            if (_sprite_renderer.flipX)
            {
                throwable = Instantiate(_throwable_prefab, _left_throwable_anchor.position, Quaternion.identity);
                ThrowableBehaviour throwable_behaviour = throwable.GetComponent<ThrowableBehaviour>();
                throwable_behaviour.Throw(Vector2.left, PlayerConstants.THROWABLE_FORCE, _player_id);
            }
            else
            {
                throwable = Instantiate(_throwable_prefab, _right_throwable_anchor.position, Quaternion.identity);
                ThrowableBehaviour throwable_behaviour = throwable.GetComponent<ThrowableBehaviour>();
                throwable_behaviour.Throw(Vector2.right, PlayerConstants.THROWABLE_FORCE, _player_id);
            }
        }

        if (_knockback)
        {
            _throwable_stun_speed = Mathf.Lerp(_knockback_max_speed, 0f, (Time.time - _last_knockback_time) / PlayerConstants.KNOCKBACK_LERP_TIME);
            if (_throwable_stun_speed == 0f)
            {
                _knockback = false;
            }
        }

        if (_attack)
        {
            _attack_vector = Vector2.Lerp(_initial_attack_vector * PlayerConstants.ATTACK_MAX_SPEED, Vector2.zero, (Time.time - _last_attack_time) / PlayerConstants.ATTACK_LERP_TIME);
            if (_attack_vector.magnitude == 0f)
            {
                _animator_controller.SetBool("dash", false);
                _attack = false;
                _sword.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(_throwable_stun_speed + _dash_speed + _speed * _input_direction.x + _attack_vector.x, _rigidbody2D.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCollidingWithGround(collision.collider))
        {
            _jump_count = PlayerConstants.JUMP_COUNT;
        }
    }

    public int GetPlayerId()
    {
        return _player_id;
    }

    private bool IsCollidingWithGround(Collider2D collider2D)
    {

        if (_player_id == 1)
        {
            return collider2D.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLATFORM1) || collider2D.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.GROUND);
        }
        else
        {
            return collider2D.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLATFORM2) || collider2D.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.GROUND);
        }
    }

    public void SetEnabled(bool value)
    {
        _enabled = value;
    }

    public void Knockback(Vector2 direction, float max_speed, float impulse)
    {
        direction.Normalize();
        if (_knockback)
        {
            return;
        }
        _last_knockback_time = Time.time;
        _knockback = true;
        if (direction.x > 0f)
        {
            _knockback_max_speed = max_speed;
        }
        else
        {
            _knockback_max_speed = max_speed * -1f;
        }
        _rigidbody2D.AddForce(Vector2.up * direction.y * impulse, ForceMode2D.Impulse);
    }
}
