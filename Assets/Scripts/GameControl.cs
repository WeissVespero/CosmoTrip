using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    [SerializeField] Button _backButton;
    [SerializeField] FightManager _fightManager;
    [SerializeField] LevelData _levelData;
    [SerializeField] Image _panel;
    private float _fadeTime = 1.5f;
    private int _currentLevel;

    private void Start()
    {
        StartCoroutine(SceneFade(true));
        Subscribe();
        DefineCurrentLevel();
    }

    private void Subscribe()
    {
        _backButton.onClick.AddListener(OnBack);
        _fightManager.Outcome += SetLevelData;
    }

    private void SetLevelData(PinStatus status)
    {
        var num = DataManager.GetNumberOfLevels();
        if (status == PinStatus.Passed && _currentLevel != num)
        {
            DataManager.SetStatus(_currentLevel + 1, PinStatus.Available);
        }
        DataManager.SetStatus(_currentLevel, status);
        OnBack();
    }

    private void DefineCurrentLevel()
    {
        for (int i = 1; i < _levelData.Levels.Count + 1; i++)
        {
            if (DataManager.GetStatus(i) == PinStatus.Active)
            {
                _currentLevel = i;
                DataManager.SetEnemiesNumber(_levelData.Levels[_currentLevel - 1].EnemiesNumber);
                break;
            }
        }
    }

    private void Unsubscribe()
    {
        _backButton.onClick.RemoveAllListeners();
        _fightManager.Outcome -= SetLevelData;
    }

    private void OnBack()
    {
        Unsubscribe();
        StartCoroutine(SceneFade(false));
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
            SceneLoader.LoadScene(SceneLoader.SceneName.Map);
        }
        yield return new WaitForSeconds(1f);
        _panel.gameObject.SetActive(false);
    }
}
