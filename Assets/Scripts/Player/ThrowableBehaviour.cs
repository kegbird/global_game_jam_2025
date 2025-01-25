using UnityEngine;

public class ThrowableBehaviour : MonoBehaviour
{
    private GameManager _game_manager;
    private SpriteRenderer _sprite_renderer;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direction;
    private int _player_id;

    private void Awake()
    {
        _game_manager = GameManager.GetInstance();
        _sprite_renderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Throw(Vector2 direction, float force, int player_id)
    {
        _player_id = player_id;
        _direction = direction;

        if(direction.x > 0)
        {
            _sprite_renderer.flipX = false;
        }
        else
        {
            _sprite_renderer.flipX = true;
        }
        _player_id = player_id;
        _rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);

        Invoke("Disable", 2f);
    }

    private void Disable()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLATFORM))
        {
            PlatformBehaviour platform_behaviour = collision.gameObject.GetComponent<PlatformBehaviour>();
            _game_manager.IncreaseScore(platform_behaviour.GetScore(), _player_id);
            platform_behaviour.Explode();
        }

        if((collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER1) && _player_id == 2) || (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER2) && _player_id == 1))
        {
            PlayerController player_controller = collision.gameObject.GetComponent<PlayerController>();
            player_controller.Knockback(_direction, PlayerConstants.THROWABLE_KNOCKBACK_MAX_SPEED, 0f);
            Disable();
        }
    }
}
