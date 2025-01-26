using System.Collections;
using UnityEngine;

public class ThrowableBehaviour : MonoBehaviour
{
    private int cumulated_score;
    private GameManager _game_manager;
    private SpriteRenderer _sprite_renderer;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direction;
    private int _player_id;
    private ParticleSystem _particle_system;
    private bool _exploded;

    private void Awake()
    {
        _game_manager = GameManager.GetInstance();
        _sprite_renderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _particle_system = GetComponent<ParticleSystem>();
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
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > 20f && !_exploded)
        {
            Destroy(gameObject);
        }
    }


    private IEnumerator DisableDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLATFORM))
        {
            PlatformBehaviour platform_behaviour = collision.gameObject.GetComponent<PlatformBehaviour>();
            _game_manager.IncreaseScore(platform_behaviour.GetScore() + cumulated_score, _player_id);
            platform_behaviour.Explode(cumulated_score);
            cumulated_score++;
        }

        if((collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER1) && _player_id == 2) || (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER2) && _player_id == 1))
        {
            PlayerController player_controller = collision.gameObject.GetComponent<PlayerController>();
            player_controller.Knockback(_direction, PlayerConstants.THROWABLE_KNOCKBACK_MAX_SPEED, 0f);
            _sprite_renderer.enabled = false;
            _rigidbody2D.linearVelocity = Vector2.zero;
            _particle_system.Play();
            _exploded = true;
            StartCoroutine(DisableDelay());
        }
    }
}
