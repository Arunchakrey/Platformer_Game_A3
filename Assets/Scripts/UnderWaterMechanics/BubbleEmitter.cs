using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(ParticleSystem))]
public class BubbleEmitter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SwimController swimmer; // auto-grab if empty

    [Header("Trickle")]
    [SerializeField] float submergedRate = 12f; // bubbles/sec when underwater
    [SerializeField] float surfaceRate   = 0f;  // bubbles/sec when above

    [Header("Stroke Burst")]
    [SerializeField] int   burstCount = 10;     // bubbles per stroke
    [SerializeField] float burstSpreadAngle = 12f;

    ParticleSystem ps;
    ParticleSystem.EmissionModule em;

    void Awake()
    {
        if (!swimmer) swimmer = GetComponentInParent<SwimController>();
        ps = GetComponent<ParticleSystem>();
        em = ps.emission;
    }

    void Update()
    {
        bool under = swimmer ? swimmer.IsSubmerged : true;
        var rate = em.rateOverTime;
        rate.constant = under ? submergedRate : surfaceRate;
        em.rateOverTime = rate;
    }

    // Call this from SwimController when a stroke happens
    public void OnStroke()
    {
        if (!ps) return;

        // Small randomized burst
        var emitParams = new ParticleSystem.EmitParams();
        // randomize direction slightly
        Vector3 dir = Quaternion.Euler(0, 0, Random.Range(-burstSpreadAngle, burstSpreadAngle)) * Vector3.up;
        emitParams.velocity = dir * Random.Range(0.3f, 0.7f);
        for (int i = 0; i < burstCount; i++)
            ps.Emit(emitParams, 1);
    }
}
