using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // resolution -- num instantiated prefabs
    [Range(10, 100)]
    public int resolution = 10;
    // prefab representing graph points
    public Transform pointPrefab;

    Transform[] points;

    void Awake() {
        // fit resolution objects within 2f units
        float step = 2f / resolution;
        // scale by same factor so resoltion prefabs fit from -1 to 1
        Vector3 scale = Vector3.one * step;
        Vector3 position = Vector3.zero;

        points = new Transform[resolution];
        for (int i = 0; i < points.Length; i++) {
            Transform point = Instantiate(pointPrefab);
            // shift half a cube to the right, scale spacing and center at 0
            position.x = (i + 0.5f) * step - 1f;
            // position.y = position.x; // y = x
            // position.y = position.x * position.x;// y = x^2
            point.localPosition = position;
            point.localScale = scale;
            // org hierarchy
            // don't keep at original pos, rot and scale
            point.SetParent(transform, false);

            points[i] = point;
        }
    }

    void Update() {
        for(int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            // position.y = position.x; // y = x
            // position.y = position.x * position.x;// y = x^2
            position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
            point.localPosition = position;
        }
    }
}
