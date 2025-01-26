using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    [SerializeField]
    private TextMeshProUGUI _player_1_score;
    [SerializeField]
    private TextMeshProUGUI _player_2_score;
    [SerializeField]
    private TextMeshProUGUI _time;
    [SerializeField]
    private TextMeshProUGUI _message;
    [SerializeField]
    private Animator _ui_animator;
    [SerializeField]
    private Image _black_screen;

    private void Awake()
    {
        _instance = this;
    }

    public void PlayCooldownAnimation(int player_id, string icon, float speed)
    {
        _ui_animator.SetTrigger(icon +"_"+ player_id);
        _ui_animator.SetFloat(icon + "_" + player_id + "_speed", speed);
    }

    public static UIManager GetInstance()
    {
        return _instance;
    }

    public void SetMessageText(string message_text)
    {
        _message.text = message_text;
    }

    public void SetTimerText(string time)
    {
        _time.text = time;
    }

    public void SetPlayerScore(int player_id, int score)
    {
        if (player_id == 1)
        {
            _player_1_score.text = string.Format("Player 1: {0}", score);
        }
        else
        {
            _player_2_score.text = string.Format("Player 2: {0}", score);
        }
    }

    public IEnumerator HideBlackScreen()
    {
        _black_screen.raycastTarget = true;
        for (float i = 1f; i >= 0; i -= Time.deltaTime)
        {
            _black_screen.color = new Color(0, 0, 0, i / 1f);
            yield return new WaitForEndOfFrame();
        }
        _black_screen.color = new Color(0, 0, 0, 0f);
        _black_screen.raycastTarget = false;
    }

    public IEnumerator ShowBlackScreen()
    {
        _black_screen.raycastTarget = true;
        for (float i = 0; i <= 1f; i += Time.deltaTime)
        {
            _black_screen.color = new Color(0, 0, 0, i / 1f);
            yield return new WaitForEndOfFrame();
        }
        _black_screen.color = new Color(0, 0, 0, 1f);
        _black_screen.raycastTarget = false;
    }
}
