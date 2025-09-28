using NUnit.Framework;
using UnityEngine;

public class OxygenDamageOverTime : MonoBehaviour
{
    [SerializeField] OxygenSystem oxygen;
    [SerializeField] Health health;
    [SerializeField] float tickEverySeconds = 1.5f;
    [SerializeField] int damagePerTick = 1;

    float timer;

    void Awake()
    {
        if (!oxygen) oxygen = GetComponent<OxygenSystem>();
        if (!health) health = GetComponent<Health>();
    }

    void Update()
    {
        if (!oxygen || !health) return;

        // if we have any o2 or are already dead, don't tick damage
        if (oxygen.CurrentOxygen > 0f || health.CurrentHearts <= 0)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;
        if (timer >= tickEverySeconds)
        {
            timer = 0f;
            health.Damage(damagePerTick);
        }
    }
}