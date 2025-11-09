using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Pin _pinPrefab;
    [SerializeField] private LevelData _levelData;
    [SerializeField] private Transform _levelsParent;
    [SerializeField] private List<GameObject> _levels = new List<GameObject>();
    [SerializeField] private Image _panel;

    private float _fadeTime = 1.5f;

    private void Start()
    {
        StartCoroutine(SceneFade(true));
        DataManager.Initialize(_levelData.Levels);
        InitializeLevels();
    }

    private void InitializeLevels()
    {
        for (int i = 0; i < _levels.Count; i++)
        {
            InstantinatePin(_levelData.Levels[i].LevelID);
        }
    }

    private IEnumerator SceneFade(bool isFadeIn) // true - scene is fade in
    {
        if (!_panel.gameObject.activeInHierarchy)
        {
            _panel.gameObject.SetActive(true);
        }
        var timer = _fadeTime;
        var color = _panel.color;
        while (timer > 0)
        {
            var fadeLevel = timer / _fadeTime;
            color.a = isFadeIn ? fadeLevel : 1 - fadeLevel;
            _panel.color = color;
            timer -= Time.deltaTime;
            yield return null;
        }
        color.a = !isFadeIn ? 1 : 0;
        _panel.color = color;
        if (!isFadeIn)
        {
            SceneLoader.LoadScene(SceneLoader.SceneName.Game);
        }        
        yield return new WaitForSeconds(1f);
        _panel.gameObject.SetActive(false);
    }

    private void InstantinatePin(int levelID)
    {
        var pin = Instantiate(_pinPrefab, _levels[levelID - 1].transform);
        pin.Initialize(levelID, DataManager.GetStatus(levelID));
        pin.OnPinClick += PinPressed;
    }

    private void PinPressed(int levelID)
    {
        //_levelData.Levels[levelID - 1].SetStatus(PinStatus.Active);

        DataManager.SetStatus(levelID, PinStatus.Active);
        StartCoroutine(SceneFade(false));
    }
}
