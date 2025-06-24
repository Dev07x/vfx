//using System.Collections.Generic;
//using UnityEngine;

//// Removed duplicate 'ExecuteAlways' attribute  
//[ExecuteAlways]
//public class SwordMotionPath : MonoBehaviour
//{
//    [Header("Recording Settings")]
//    public int maxPoints = 100;
//    public float recordInterval = 0.02f;

//    [SerializeField]
//    private List<Vector3> pathPoints = new List<Vector3>();

//    private float timeSinceLastRecord = 0f;

//    private void Update()
//    {
//        // Only record during Play mode  
//        if (Application.isPlaying)
//        {
//            timeSinceLastRecord += Time.deltaTime;

//            if (timeSinceLastRecord >= recordInterval)
//            {
//                timeSinceLastRecord = 0f;
//                RecordPoint();
//            }
//        }
//    }

//    void RecordPoint()
//    {
//        pathPoints.Add(transform.position);

//        if (pathPoints.Count > maxPoints)
//        {
//            pathPoints.RemoveAt(0);
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;

//        for (int i = 1; i < pathPoints.Count; i++)
//        {
//            Gizmos.DrawLine(pathPoints[i - 1], pathPoints[i]);
//        }
//    }
//}
