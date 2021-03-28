using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPointsRailControl : MonoBehaviour
{
    public List<Transform> listRailPoints;
    private PointsRailControll pointsRailControll;
    public LayerMask layerMask;
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
        //int randomIndex = 0;
        Transform outTransform = listRailPoints[0];
        if (curretnUsedIndex == 0)
        {
            //randomIndex = Random.Range(4, 5);
            outTransform = ChooseBestPointBeetwenTwo(pointsRailControll.listRailPoints[4], pointsRailControll.listRailPoints[5]);
        }
        else if (curretnUsedIndex == 1)
        {
            //randomIndex = Random.Range(2, 3);
            outTransform = ChooseBestPointBeetwenTwo(pointsRailControll.listRailPoints[2], pointsRailControll.listRailPoints[3]);
        }
        else
        {
            //randomIndex = Random.Range(0,1);
            outTransform = ChooseBestPointBeetwenTwo(pointsRailControll.listRailPoints[0], pointsRailControll.listRailPoints[1]);
        }
        //outTransform = pointsRailControll.listRailPoints[randomIndex];
        
        newNormalIndex = pointsRailControll.listRailPoints.FindIndex(x=>x == outTransform);
        Debug.Log(newNormalIndex);
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

    private Transform ChooseBestPointBeetwenTwo(Transform point1, Transform point2)
    {
        Vector3[] positionsOfTwoPoints = new Vector3[] { point1.position, point2.position };
        int bestPointIndex = 0;
        float biggestDistance = 1000f;
        for (int i = 0; i < positionsOfTwoPoints.Length; i++)
        {
            Vector3 direction = new Vector3(positionsOfTwoPoints[i].x, positionsOfTwoPoints[i].y+1f, positionsOfTwoPoints[i].z + 10f) - positionsOfTwoPoints[i];
            Vector3 orign = new Vector3(positionsOfTwoPoints[i].x, positionsOfTwoPoints[i].y + 1f, positionsOfTwoPoints[i].z);
            RaycastHit hit;
            Physics.Raycast(orign, direction, out hit, layerMask);
            if (hit.distance < biggestDistance)
            {
                biggestDistance = hit.distance;
                bestPointIndex = i;
            }
        }

        if (bestPointIndex == 0)
        {
            return point1;
        }
        else
        {
            return point2;
        }
    }
}
