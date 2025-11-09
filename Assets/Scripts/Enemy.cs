using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : Character, IPointerClickHandler
{
    public event Action<Character> OnCharacterClick;
    public event Action OnEnemyDead;
    public event Action OnRandomEvent;

    [SerializeField] private Image _activeIndicator;
    [SerializeField] private Image _image;



    public void OnPointerClick(PointerEventData eventData)
    {
        print($"Character {name} pressed");
        OnCharacterClick?.Invoke(this);
    }

    public override void DeathInvoke()
    {
        OnEnemyDead?.Invoke();
    }

    public void EraseCharacter()
    {
        StartCoroutine(SmoothErase());
    }

    private IEnumerator SmoothErase()
    {
        var timer = 1f; //исчезновение за 1 секунду
        var color = _image.color;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            color.a = timer;
            _image.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void SetActiveToBeHitten()
    {
        _activeIndicator.gameObject.SetActive(true);
    }

    public void SetNotActiveToBeHitten()
    {
        _activeIndicator.gameObject.SetActive(false);
    }

    public override void HealthRecover()
    {
        int randNumHealthRecover = UnityEngine.Random.Range(1, 5); // ѕротивники при атаке имеют шанс 25% восстановить часть хп.
        if (randNumHealthRecover == 1)
        {
            Health += Health / 2;
            HealthBarUpdate();
            StartCoroutine(ShowStatusText("Health recovered"));
        }
    }

    public override void Stun()
    {
        int randNumStun = UnityEngine.Random.Range(1, 4); // кажда€ атака игрока имеет шанс 30% оглушить цель на 2 секунды 
        if(randNumStun == 1)
        {
            StartCoroutine(ShowStatusText("Stunned"));
            OnRandomEvent?.Invoke();
        }
    }
}
