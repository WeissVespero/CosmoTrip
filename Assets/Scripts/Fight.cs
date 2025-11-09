using System.Collections;
using UnityEngine;

public class Fight : MonoBehaviour
{
    private Character _victimToFight;
    [SerializeField] private Character _me;
    private bool _isPaused;
    private float _pauseTime = 2f; // время оглушения врага

    [SerializeField] private Ability _ability1;
    [SerializeField] private Ability _ability2;

    private float _ability1ManaCost = 20;
    private float _ability2ManaCost = 30;

    private void Start()
    {
        Subscribe();
        StartCoroutine(HitEmAll());
    }

    private void Subscribe()
    {
        if (_me is Enemy)
        {
            (_me as Enemy).OnRandomEvent += () => _isPaused = true;
        }
        if (_me is Player)
        {
            _ability1.OnAbilityApply += Ability1;
            _ability2.OnAbilityApply += Ability2;
        }
    }

    private void Ability1()
    {
        var character = _me as Player; //!!!!!!!!!!!!!
        if (character != null && character.Mana >= _ability1ManaCost)
        {
            character.ManaChange(-20);
            _me.AnimateMe("Hit");
            _victimToFight.Damage(_me.AttackForce);
        }
        
    }

    private void Ability2()
    {
        (_me as Player).ManaChange(-30); // предусмотреть несрабатывание абилки при недостатке маны
        StartCoroutine(AbilityTimer());
    }

    private IEnumerator AbilityTimer()
    {
        var attackForce = _me.AttackForce;
        var attackSpeed = _me.AttackSpeed;
        _me.AttackForce *= 2f;
        _me.AttackSpeed *= .6f;
        yield return new WaitForSeconds(10f);
        _me.AttackForce = attackForce;
        _me.AttackSpeed = attackSpeed;
    }

    public void ChangeVictim(Character newVictim)
    {
        _victimToFight = newVictim;
    }

    private IEnumerator HitEmAll()
    {
        while (_me.Health > 0)
        {
            var timer = 0f;
            var pauseTimer = 0f;
            while (timer < _me.AttackSpeed)
            {
                while (_isPaused)
                {
                    pauseTimer += Time.deltaTime;
                    if (pauseTimer > _pauseTime) _isPaused = false;
                    yield return null;
                }
                timer += Time.deltaTime;
                yield return null;
            }
            _me.AnimateMe("Hit");
            _victimToFight.Damage(_me.AttackForce);
            yield return null;


            _me.HealthRecover();
        }
    }
}
