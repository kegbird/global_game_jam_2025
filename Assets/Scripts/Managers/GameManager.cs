using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField]
    private float _round_time;
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
    GameSoundsManager _game_sounds_manager;
    [SerializeField]
    private GameObject _upper_limit;

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
        _game_sounds_manager = GameSoundsManager.GetInstance();
        _player_1_transform = GameObject.FindGameObjectWithTag("Player1").transform;
        _player_2_transform = GameObject.FindGameObjectWithTag("Player2").transform;
        _player_1_controller = _player_1_transform.GetComponent<PlayerController>();
        _player_2_controller = _player_2_transform.GetComponent<PlayerController>();
        _player_1_controller.SetGravityScale(0f);
        _player_2_controller.SetGravityScale(0f);
        _game_over = false;
        StartCoroutine(_ui_manager.HideBlackScreen());
        StartCoroutine(DelayBeforeStartCoroutine());
    }

    private IEnumerator DelayBeforeStartCoroutine()
    {
        _ui_manager.SetTimerText(_round_time.ToString());
        _ui_manager.SetMessageText("Prepare yourself...");
        yield return new WaitForSeconds(3f);
        _player_1_controller.SetGravityScale(2f);
        _player_2_controller.SetGravityScale(2f);
        _player_1_controller.SetEnabled(true);
        _player_2_controller.SetEnabled(true);
        StartCoroutine(GameManagerCoroutine());
        _ui_manager.SetMessageText("Go!");
        yield return new WaitForSeconds(2f);
        _ui_manager.SetMessageText("");
    }

    private IEnumerator GameManagerCoroutine()
    {   
        while (!_game_over)
        {
            yield return new WaitForSeconds(1f);
            _round_time -= 1f;
            _ui_manager.SetTimerText(_round_time.ToString());

            if(_round_time == 0)
            {
                _game_over = true;
                if(_player_1_score> _player_2_score)
                {
                    _ui_manager.SetMessageText("Player 1 Wins");
                    GameOverManager.PLAYER_WINNER = 1;
                }
                else if(_player_2_score > _player_1_score)
                {
                    _ui_manager.SetMessageText("Player 2 Wins");
                    GameOverManager.PLAYER_WINNER = 2;
                }
                else
                {
                    _ui_manager.SetMessageText("Draw");
                    GameOverManager.PLAYER_WINNER = Random.Range(0, 2);
                }
            }
            if (_player_1_transform.position.y < _player_deadline.position.y)
            {
                _game_over = true;
                _ui_manager.SetMessageText("Player 2 Wins");
                GameOverManager.PLAYER_WINNER = 2;
            }
            else if(_player_2_transform.position.y < _player_deadline.position.y)
            {
                _game_over = true;
                _ui_manager.SetMessageText("Player 1 Wins");
                GameOverManager.PLAYER_WINNER = 1;
            }
        }

        _player_1_controller.SetEnabled(false);
        _player_2_controller.SetEnabled(false);
        yield return new WaitForSeconds(2f);
        _game_sounds_manager.StopCalderonAndTheme();
        yield return StartCoroutine(_ui_manager.ShowBlackScreen());
        //SceneManager.LoadScene(Constants.GAME_OVER_SCENE);
        SceneManager.LoadScene(Constants.GAME_SCENE);
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

    public float GetRoundTime()
    {
        return _round_time;
    }
}
