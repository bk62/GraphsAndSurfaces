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

    public GraphFunctionName function;
    static GraphFunction[] functions = {
        SineFunction, MultiSineFunction
    };

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
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        for(int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = f(position.x, t);
            point.localPosition = position;
        }
    }

    static float SineFunction(float x, float t, float z = 0) {
        return Mathf.Sin(Mathf.PI * (x + t));
    }

    static float MultiSineFunction(float x, float t, float z = 0) {
        float y = Mathf.Sin(Mathf.PI * (x + t));
        // add another sine wave with half the freq
        // and amplitude
        y += Mathf.Sin(2f * Mathf.PI * (x + t)) / 2f;
        // amplitude of resulting function is 3/2
        // normalize
        y *= 2f / 3f;
        return y;
    }
}

