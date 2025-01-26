using System.Collections;
using UnityEngine;

public class GameSoundsManager : MonoBehaviour
{
    private static GameSoundsManager _instance;
    [SerializeField]
    private AudioSource _calderon_audio_source;
    [SerializeField]
    private AudioSource _theme_audio_source;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        PlayCalderonAndTheme();
    }

    public static GameSoundsManager GetInstance()
    {
        return _instance;
    }

    public void PlayCalderonAndTheme()
    {
        StartCoroutine(StartMusic());
    }

    public void StopCalderonAndTheme()
    {
        StartCoroutine(StopMusic());
    }

    private IEnumerator StartMusic()
    {
        _calderon_audio_source.Play();
        _theme_audio_source.Play();
        float i = 0f;
        while (i <= 1f)
        {
            i += Time.deltaTime;
            _calderon_audio_source.volume = i;
            _theme_audio_source.volume = i;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator StopMusic()
    {
        float i = 0f;
        while (i <= 1f)
        {
            i += Time.deltaTime;
            _calderon_audio_source.volume = 1f - i;
            _theme_audio_source.volume = 1f - i;
            yield return new WaitForEndOfFrame();
        }
    }
}
