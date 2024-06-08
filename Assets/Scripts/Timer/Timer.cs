using System;
using System.Collections;
using UnityEngine;

public static class Timer 
{
    public static void DoTimer(this MonoBehaviour gameObject, float time, Action some)
    {
        gameObject.StartCoroutine(Rutine());
        
        
        IEnumerator Rutine()
        {
            float timer = time;

            WaitForFixedUpdate wait = new WaitForFixedUpdate();

            while (time > 0)
            {
                time -= Time.fixedDeltaTime;

                yield return wait;
            }

            some.Invoke();
        }
    }

   
}
