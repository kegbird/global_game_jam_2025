using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private float _min_platform_spawn_delay;
    [SerializeField]
    [Range(0, 20)]
    private float _max_platform_spawn_delay;
    [SerializeField]
    [Range(1, 3)]
    private float _min_platform_speed;
    [SerializeField]
    [Range(1, 10)]
    private float _max_platform_speed;
    [SerializeField]
    [Range(0, 2)]
    private float _min_platform_scale;
    [SerializeField]
    [Range(0, 2)]
    private float _max_platform_scale;
    [SerializeField]
    private Transform _left_spawning_boundary;
    [SerializeField]
    private Transform _right_spawning_boundary;
    [SerializeField]
    private GameObject _platforms_holder;
    [SerializeField]
    private List<GameObject> _platform_list;
    [SerializeField]
    private GameObject _platform;
    private GameManager _game_manager;

    void Awake()
    {
        _platform_list = new List<GameObject>();
        for (int i=0; i<_platforms_holder.transform.childCount; i++)
        {
            _platform_list.Add(_platforms_holder.transform.GetChild(i).gameObject);
        }
    }

    void Start()
    {
        _game_manager = GameManager.GetInstance();
        StartCoroutine(PlatformSpawnerCoroutine());
    }

    private IEnumerator PlatformSpawnerCoroutine()
    {
        int platform_index = 0;
        Vector2 platform_initial_position;
        while(true)
        {
            platform_initial_position = new Vector2(Random.Range(_left_spawning_boundary.position.x, _right_spawning_boundary.position.x), transform.position.y + Random.Range(-1, 1));

            GameObject generated_platform;
            PlatformBehaviour platform_behaviour;
            if (_platform_list[platform_index].activeInHierarchy)
            {
                generated_platform = Instantiate(_platform, platform_initial_position, Quaternion.identity);
                platform_behaviour = generated_platform.GetComponent<PlatformBehaviour>();
                platform_behaviour.SetIsTemporary(true);
            }
            else
            {
                generated_platform = _platform_list[platform_index];
                generated_platform.transform.position = platform_initial_position;
                platform_behaviour = generated_platform.GetComponent<PlatformBehaviour>();
            }
            platform_behaviour.GetComponent<SpriteRenderer>().enabled = true;
            platform_behaviour.GetComponent<CircleCollider2D>().enabled = true;
            platform_behaviour.SetExplode(false);
            platform_behaviour.SetScale(Random.Range(_min_platform_scale, _max_platform_scale));
            platform_behaviour.SetSpeed(Random.Range(_min_platform_speed, _max_platform_speed));
            generated_platform.SetActive(true);
            yield return new WaitForSeconds(GetDelayByRoundTime());
            platform_index++;
            platform_index %= _platform_list.Count;
        }
    }

    private float GetDelayByRoundTime()
    {
        float round_time = _game_manager.GetRoundTime();

        if(round_time > 110f)
        {
            return Random.Range(0, 0.5f);
        }
        else if (110f > round_time && round_time > 100f)
        {
            return Random.Range(0.5f, 1f);
        }
        else
        {
            return Random.Range(1f, 2f);
        }
        
    }

}
