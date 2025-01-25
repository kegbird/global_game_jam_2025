using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField]
    private Transform _player_1_transform;
    [SerializeField]
    private Transform _player_2_transform;
    [SerializeField]
    private Transform _player_deadline;
    [SerializeField]
    private bool _game_over;
    [SerializeField]
    private int _player_1_score = 0;
    [SerializeField]
    private int _player_2_score = 0;

    private UIManager _ui_manager;
    private PlayerController _player_1_controller;
    private PlayerController _player_2_controller;

    private void Awake()
    {
        _instance = this;
        _ui_manager = GetComponent<UIManager>();
    }

    private void Start()
    {
        _player_1_transform = GameObject.FindGameObjectWithTag("Player1").transform;
        _player_2_transform = GameObject.FindGameObjectWithTag("Player2").transform;
        _player_1_controller = _player_1_transform.GetComponent<PlayerController>();
        _player_2_controller = _player_2_transform.GetComponent<PlayerController>();
        _game_over = false;
        StartCoroutine(DelayBeforeStartCoroutine());
    }

    private IEnumerator DelayBeforeStartCoroutine()
    {
        _ui_manager.SetMessageText("Ready?");
        yield return new WaitForSeconds(3f);
        _player_1_controller.SetEnabled(true);
        _player_2_controller.SetEnabled(true);
        StartCoroutine(GameOverCheckerCoroutine());
        _ui_manager.SetMessageText("Go!");
        yield return new WaitForSeconds(1f);
        _ui_manager.SetMessageText("");
    }

    private IEnumerator GameOverCheckerCoroutine()
    {
        while (!_game_over)
        {
            yield return new WaitForSeconds(1f);
            if (_player_1_transform.position.y < _player_deadline.position.y)
            {
                _game_over = true;
                _ui_manager.SetMessageText("Player 2");
            }
            else if(_player_2_transform.position.y < _player_deadline.position.y)
            {
                _game_over = true;
                _ui_manager.SetMessageText("Player 1");
            }
        }
    }

    public void IncreaseScore(int amount, int player_id)
    {
        if(player_id == 1)
        {
            _player_1_score += amount;
            _ui_manager.SetPlayerScore(1, _player_1_score);
        }
        else
        {
            _player_2_score += amount;
            _ui_manager.SetPlayerScore(2, _player_2_score);
        }
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }
}
