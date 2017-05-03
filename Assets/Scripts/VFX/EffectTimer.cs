using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTimer : MonoBehaviour
{
    public float delayRemove = 0f;
    ParticleSystem ps;

    void OnEnable()
    {
        if (!ps)
            ps = GetComponent<ParticleSystem>();

        ps.Play();
    }

    void OnDisable()
    {
        if (ps)
            ps.Stop();
    }

    void Update()
    {
        //if effect has finished playing, disable it
        if (!ps.isPlaying)
        {
            transform.parent = null;
            StartCoroutine(DelayedRemove());
        }

    }

    IEnumerator DelayedRemove()
    {
        yield return new WaitForSeconds(delayRemove);
        gameObject.SetActive(false);
    }
}
