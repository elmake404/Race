using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkWithArray : MonoBehaviour
{
    public static Object[] GetMixedArray(Object[] inObject)
    {

        for (int i = inObject.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = inObject[j];
            inObject[j] = inObject[i];
            inObject[i] = temp;
        }
        return inObject;
    }

    public static Queue<Object> FillQueue(Object[] toFilled)
    {
        Queue<Object> newQueue = new Queue<Object>();
        for (int i = 0; i < toFilled.Length; i++)
        {
            newQueue.Enqueue(toFilled[i]);
        }

        return newQueue;
    }

}
