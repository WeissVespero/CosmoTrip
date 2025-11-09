using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [SerializeField] private Button _button;

    [SerializeField] private Image _abilBarFill;

    public float _abilTimeRecover;

    public event Action OnAbilityApply;


    private void Start()
    {
        Subscribe();
        StartCoroutine(AbilityRecover());
    }

    private void Subscribe()
    {
        _button.onClick.AddListener(OnPressAbil);
    }

    private void OnPressAbil()
    {
        OnAbilityApply?.Invoke();
        _button.interactable = false;
        _abilBarFill.fillAmount = 0f;
        StartCoroutine(AbilityRecover());
    }

    private IEnumerator AbilityRecover()
    {
        var timer = 0f;
        while (timer < _abilTimeRecover)
        {
            timer += Time.deltaTime;
            _abilBarFill.fillAmount = timer / _abilTimeRecover;
            yield return null;
        }
        yield return null;
        _button.interactable = true;
        _abilBarFill.fillAmount = 1f;
        
    }

    private void Unsubscribe()
    {
        _button.onClick.RemoveAllListeners();
    }
}
