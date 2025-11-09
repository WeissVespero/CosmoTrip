using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public float MaxMana;
    public float Mana;

    private float _manaTimeRecover = 1f;
    private float _manaRecoverTimer = 0f;

    [SerializeField] private Image _magicBarFill;

    public event Action OnPlayerDead;

    private void Start()
    {
        ManaBarUpdate();
    }

    private void Update()
    {
        if (Mana < MaxMana)
        {
            _manaRecoverTimer += Time.deltaTime;
            if (_manaRecoverTimer > _manaTimeRecover)
            {
                _manaRecoverTimer = 0;
                ManaChange(MaxMana / 10); //increase for 10 percent
            }
        }
    }

    public override void DeathInvoke()
    {
        OnPlayerDead?.Invoke();
    }

    private void ManaBarUpdate()
    {
        _magicBarFill.fillAmount = Mana / MaxMana;
    }

    public void ManaChange(float mana)
    {
        Mana += mana;
        if (Mana <= 0)
        {
            //death of character
            Mana = 0;
        }
        if (Mana > MaxMana)
        {
            Mana = MaxMana;
        }
        ManaBarUpdate();
    }
}
