using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FightManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Fight _playerFight;
    private Enemy[] _enemies;
    private Fight[] _enemyFights;
    [SerializeField] private Transform[] _spawnPlaces;
    [SerializeField] private GameObject _enemyPrefab;
    private int _enemyCounter;
    private int _numVisibleEnemies = 3;
    private int _currentEnemy;

    public event Action<PinStatus> Outcome;

    private void Start()
    {
        _enemyCounter = DataManager.GetEnemiesNumber();
        Initialize();
    }

    private void Initialize()
    {
        _enemies = new Enemy[_numVisibleEnemies];
        _enemyFights = new Fight[_numVisibleEnemies];

        for (int i = 0; i < _spawnPlaces.Length; i++)
        {
            var enemy = Instantiate(_enemyPrefab, _spawnPlaces[i]);
            _enemies[i] = enemy.GetComponent<Enemy>();
            _enemyFights[i] = enemy.GetComponent<Fight>();
        }

        _enemies[0].SetActiveToBeHitten();
        _playerFight.ChangeVictim(_enemies[0]);
        _currentEnemy = 0;
        Subscribe();
    }

    private void Subscribe()
    {
        _player.OnPlayerDead += PlayerDeath;
        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].OnCharacterClick += ChooseEnemyToHit;
            _enemies[i].OnEnemyDead += EnemyDeath;
            _enemyFights[i].ChangeVictim(_player);
        }
    }

    private void Unsubscribe()
    {
        _player.OnPlayerDead -= PlayerDeath;
        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].OnCharacterClick -= ChooseEnemyToHit;
            _enemies[i].OnEnemyDead -= EnemyDeath;
        }
    }
   
    private IEnumerator enemySpawn()
    {
        bool isEmptySpaceForEnemy = false;
        while (!isEmptySpaceForEnemy)
        {
            for (int i = 0; i < _enemies.Length; i++)
            {
                if(_enemies[i] == null)
                {
                    isEmptySpaceForEnemy = true;
                    yield return new WaitForSeconds(2); // spawn new enemy after 2 seconds
                    var enemy = Instantiate(_enemyPrefab, _spawnPlaces[i]);
                    _enemies[i] = enemy.GetComponent<Enemy>();
                    _enemyFights[i] = enemy.GetComponent<Fight>();
                    _enemies[i].IsActive = true;
                    _enemies[i].OnCharacterClick += ChooseEnemyToHit;
                    _enemies[i].OnEnemyDead += EnemyDeath;
                    _enemyFights[i].ChangeVictim(_player);
                    break;
                }
            }
            yield return null;
        }
    }

    private void ShiftEnemy()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i].IsActive)
            {
                _enemies[i].SetActiveToBeHitten();
                _playerFight.ChangeVictim(_enemies[i]);
                _currentEnemy = i;
                return;
            }
        }
    }

    public void EnemiesCheck()
    {
        _numVisibleEnemies--;
        if (_enemyCounter - _numVisibleEnemies > 0)
        {
            StartCoroutine(enemySpawn());
            _numVisibleEnemies++;
        }
    }

    private void EnemyDeath()
    {
        _enemyCounter--;
        _enemies[_currentEnemy].OnCharacterClick -= ChooseEnemyToHit;
        _enemies[_currentEnemy].OnEnemyDead -= EnemyDeath;
        _enemies[_currentEnemy].SetNotActiveToBeHitten();
        _enemies[_currentEnemy].IsActive = false;
        _enemies[_currentEnemy].EraseCharacter();
        ShiftEnemy();
        EnemiesCheck();
        if (_enemyCounter != 0)
        {
            return;
        }
        Outcome.Invoke(PinStatus.Passed);
        Unsubscribe();
    }

    private void PlayerDeath()
    {
        Outcome.Invoke(PinStatus.Available);
        Unsubscribe();
    }

    private void ChooseEnemyToHit(Character character)
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i].name == character.name)
            {
                _enemies[i].SetActiveToBeHitten();
                _playerFight.ChangeVictim(character);
                _currentEnemy = i;
            }
            else
            {
                _enemies[i].SetNotActiveToBeHitten();
            }
        }
    }
}
