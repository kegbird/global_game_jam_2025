using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static int PLAYER_WINNER;
    [SerializeField]
    private Image _black_screen;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private TextMeshProUGUI _player;
    [SerializeField]
    private AudioSource _audio_source;

    private void Start()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public IEnumerator GameOverCoroutine()
    {
        _audio_source.Play();
        if(PLAYER_WINNER == 1)
        {
            _player.text = "<wave a=0.1>Player 1 Wins";
            _animator.SetTrigger("p1");
        }
        else
        {
            _player.text = "<wave a=0.1>Player 2 Wins";
            _animator.SetTrigger("p2");
        }
        yield return StartCoroutine(HideBlackScreen());

        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(ShowBlackScreen());
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_INDEX);
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
