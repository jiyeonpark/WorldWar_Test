using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class MonoBehaviourExtension
{
    public static void After(this MonoBehaviour behaviour, float wait, Action f)
    {
        behaviour.StartCoroutine(AfterCR(wait, f));
    }

    static IEnumerator AfterCR(float wait, Action f)
    {
        yield return new WaitForSeconds(wait);
        f();
    }

    public static void YieldAndExecute(this MonoBehaviour behaviour, Action f)
    {
        behaviour.StartCoroutine(YieldAndExecuteCR(f));
    }

    static IEnumerator YieldAndExecuteCR(Action f)
    {
        yield return null;

        f();
    }
}
