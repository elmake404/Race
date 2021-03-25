using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusAction : MonoBehaviour
{
    CarPropereties carPropereties;

    private void OnEnable()
    {
        carPropereties = transform.GetComponent<CarPropereties>();
    }

    IEnumerator ActionBonus()
    {
        yield return null;
    }
}
