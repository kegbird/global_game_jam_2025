using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int _player_id;
    [SerializeField]
    [Range(0f, 100f)]
    private float _speed = 1f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _input_direction;
    private string _horizontal_axis;
    private string _vertical_axis;
    private string _jump_key;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _horizontal_axis = "Horizontal" + _player_id;
        _vertical_axis = "Vertical" + _player_id;
        _jump_key = "Jump" + _player_id;
    }

    private void Update()
    {
        _input_direction = new Vector2(Input.GetAxis(_horizontal_axis), 0f);
        Debug.Log(_input_direction);
    }

    private void FixedUpdate()
    {

        _rigidbody2D.linearVelocity = new Vector2(_speed * _input_direction.x, _rigidbody2D.linearVelocity.y);
    }
}
