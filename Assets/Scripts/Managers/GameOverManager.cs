using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static int PLAYER_WINNER;
    [SerializeField]
    private Image _black_screen;
    [SerializeField]
    private Image _player_1_win;
    [SerializeField]
    private Image _player_2_win;

    private void Start()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public IEnumerator GameOverCoroutine()
    {
        if(PLAYER_WINNER == 1)
        {
            _player_1_win.enabled = true;
        }
        else
        {
            _player_2_win.enabled = true;
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
