using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsRailControll : MonoBehaviour
{
    public List<Transform> listRailPoints;
    //private PlayerCarController playerCar;

    private void Start()
    {

    }

    public Transform GetNearRail(int currentPos, int sideLeftRight)
    {
        Transform outNewPos;
        int changeIndex = currentPos + sideLeftRight;
        Debug.Log(changeIndex);

        if (changeIndex > (listRailPoints.Count - 1) | changeIndex < 0)
        {
            return null;
        }

        else
        {
            outNewPos = listRailPoints[changeIndex];

            return outNewPos;
        }
        
    }
}
