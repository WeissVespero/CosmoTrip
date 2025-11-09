using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public bool IsActive = true;

    public float MaxHealth;
    public float Health;

    public float AttackForce;
    public float AttackSpeed;

    private float DamageResist = 1; //coefficient of resistence. If 1, damage = attack force.

    

    [SerializeField] private Image _healthBarFill;
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _statusText;


    public void AnimateMe(string action)
    {
        _animator.SetTrigger(action);
    }

    public void HealthBarUpdate()
    {
        _healthBarFill.fillAmount = Health / MaxHealth;
    }

    public void Damage(float damage)
    {
        Health -= damage * DamageResist;
        Stun();
        AnimateMe("Injure");
        if (Health <= 0)
        {
            //death of character
            Health = 0;
            DeathInvoke();
        }
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        HealthBarUpdate();
    }

    public virtual void DeathInvoke()
    {
    }

    public virtual void HealthRecover()
    {

    }

    public virtual void Stun()
    {

    }

    public IEnumerator ShowStatusText(string text)
    {
        _statusText.text = text;
        yield return new WaitForSeconds(2f);
        _statusText.text = "";
        yield return null;
    }
}
