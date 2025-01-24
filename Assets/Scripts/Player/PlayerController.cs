using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int _player_id;
    [SerializeField]
    [Range(0f, 20f)]
    private float _speed = 1f;
    [SerializeField]
    [Range(0f, 20f)]
    private float _jump_impulse_force = 1f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _input_direction;
    private string _horizontal_axis;
    private KeyCode _jump_key;
    private int _jump_count;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _horizontal_axis = "Horizontal" + _player_id;
        _jump_key = _player_id == 1 ? KeyCode.Joystick1Button0 : KeyCode.Joystick2Button0;
        _jump_count = PlayerConstants.JUMP_COUNT;
    }

    private void Update()
    {
        _input_direction = new Vector2(Input.GetAxis(_horizontal_axis), 0f);
        if(_jump_count > 0 && Input.GetKeyDown(_jump_key))
        {
            _jump_count--;
            _rigidbody2D.AddForce(Vector2.up * _jump_impulse_force, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(_speed * _input_direction.x, _rigidbody2D.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.GROUND))
        {
            _jump_count = PlayerConstants.JUMP_COUNT;
        }
    }
}
