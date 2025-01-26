using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private Image _black_screen;
    [SerializeField]
    private VideoPlayer _video_player;

    private void Start()
    {
        StartCoroutine(HideBlackScreen());
        Invoke("ChangeScene", 15f);
    }

    private IEnumerator HideBlackScreen()
    {
        yield return new WaitUntil(() => _video_player.isPrepared);
        _black_screen.raycastTarget = true;
        for (float i = 1f; i >= 0; i -= Time.deltaTime)
        {
            _black_screen.color = new Color(0, 0, 0, i / 1f);
            yield return new WaitForEndOfFrame();
        }
        _black_screen.color = new Color(0, 0, 0, 0f);
        _black_screen.raycastTarget = false;
    }

    private void ChangeScene()
    {
        StartCoroutine(ShowBlackScreen());
    }

    private IEnumerator ShowBlackScreen()
    {
        _black_screen.raycastTarget = true;
        for (float i = 0; i <= 1f; i += Time.deltaTime)
        {
            _black_screen.color = new Color(0, 0, 0, i / 1f);
            yield return new WaitForEndOfFrame();
        }
        _black_screen.color = new Color(0, 0, 0, 1f);
        _black_screen.raycastTarget = false;
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_INDEX);
    }
}
