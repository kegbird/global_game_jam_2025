using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private int _player_id;
    private GameManager _game_manager;

    private void Start()
    {
        PlayerController player_controller = GetComponentInParent<PlayerController>();
        _player_id = player_controller.GetPlayerId();
        _game_manager = GameManager.GetInstance();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLATFORM))
        {
            PlatformBehaviour platform_behaviour = collision.gameObject.GetComponent<PlatformBehaviour>();
            _game_manager.IncreaseScore(platform_behaviour.GetScore(), _player_id);
            platform_behaviour.Explode();
        }
        if ((collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER1) && _player_id == 2) || (collision.gameObject.layer == LayerMask.NameToLayer(LayerMaskNames.PLAYER2) && _player_id == 1))
        {
            PlayerController player_controller = collision.gameObject.GetComponent<PlayerController>();
            player_controller.Knockback(transform.right, PlayerConstants.ATTACK_MAX_SPEED, PlayerConstants.ATTACK_IMPULSE);
        }
    }
}
