using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPointsRailControl : MonoBehaviour
{
    public List<Transform> listRailPoints;
    private PointsRailControll pointsRailControll;
    private void Start()
    {
        pointsRailControll = FindObjectOfType<PointsRailControll>();
    }

    public Transform GetInitialStartBonusPoint(Transform atPoint, out int initialIndex)
    {
        initialIndex = 0;
        Transform newNearestPoint = listRailPoints[0];
        float cuttestDistance = 1000f;
        for (int i = 0; i < listRailPoints.Count; i++)
        {
            if (Vector3.Distance(listRailPoints[i].position, atPoint.position) < cuttestDistance)
            {
                cuttestDistance = Vector3.Distance(listRailPoints[i].position, atPoint.position);
                newNearestPoint = listRailPoints[i];
                initialIndex = i;
            }
        }
        return newNearestPoint;
    }


    public Transform GetOutNormalRailPoint(int curretnUsedIndex, out int newNormalIndex)
    {
        int randomIndex = 0;
        Transform outTransform = listRailPoints[0];
        if (curretnUsedIndex == 0)
        {
            randomIndex = Random.Range(4, 6);
        }
        else if (curretnUsedIndex == 1)
        {
            randomIndex = Random.Range(2, 4);
        }
        else
        {
            randomIndex = Random.Range(0,2);
            
        }
        outTransform = pointsRailControll.listRailPoints[randomIndex];
        newNormalIndex = randomIndex;
        return outTransform;
    }

    public Transform GetNearPoint( int direction, int currentIndex)
    {
        int newIndex = currentIndex + direction;
        if (newIndex < 0 | newIndex > listRailPoints.Count-1)
        {
            return null;
        }
        else
        {
            return listRailPoints[newIndex];
        }
    }
}
