using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _player_1_score;
    [SerializeField]
    private TextMeshProUGUI _player_2_score;

    [SerializeField]
    private TextMeshProUGUI _message;

    public void SetMessageText(string message_text)
    {
        _message.text = message_text;
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
}
