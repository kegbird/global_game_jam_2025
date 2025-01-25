using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform _player_1;
    [SerializeField]
    private Transform _player_2;
    [SerializeField]
    private Transform _player_deadline;
    [SerializeField]
    private bool _game_over;

    private void Start()
    {
        _game_over = false;
        StartCoroutine(GameOverCheckerCoroutine());
    }

    private IEnumerator GameOverCheckerCoroutine()
    {
        while (!_game_over)
        {
            yield return new WaitForSeconds(1f);
            if (_player_1.position.y < _player_deadline.position.y || _player_2.position.y < _player_deadline.position.y)
            {
                _game_over = true;
            }
        }
        Debug.Log("Game Over");
    }
}
