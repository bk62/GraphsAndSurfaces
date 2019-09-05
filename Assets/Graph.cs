using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    const float pi = Mathf.PI;

    [Range (10, 100)]
    public int resolutionX = 10;
    [Range (10, 100)]
    public int resolutionZ = 1;

    // prefab representing graph points
    public Transform pointPrefab;

    Transform[] points;

    public GraphFunctionName function;
    static GraphFunction[] functions = {
        SineFunction,
        MultiSineFunction
    };

    void Awake () {
        // fit resolution objects within 2f units
        float stepX = 2f / resolutionX;
        float stepZ = 2f / resolutionZ;
        // scale by same factor so resoltion prefabs fit from -1 to 1
        Vector3 scaleX = Vector3.one * stepX;
        Vector3 scaleZ = Vector3.one * stepZ;

        Vector3 position = Vector3.zero;

        points = new Transform[resolutionX * resolutionZ];
        for (int i = 0, z = 0; z < resolutionZ; z++) {
            position.z = (z + 0.5f) * stepZ - 1f;
            for (int x = 0; x < resolutionX; x++, i++) {
                Transform point = Instantiate (pointPrefab);
                // shift half a cube to the right, scale spacing and center at 0
                position.x = (x + 0.5f) * stepX - 1f;
                
                point.localPosition = position;
                // TODO
                point.localScale = resolutionX > resolutionZ ? scaleX : scaleZ; 
                // point.localScale.z = scaleZ;
                // org hierarchy
                // don't keep at original pos, rot and scale
                point.SetParent (transform, false);

                points[i] = point;
            }
        }
    }

    void Update () {
        float t = Time.time;
        GraphFunction f = functions[(int) function];
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            Vector2 xz = new Vector2 (position.x, position.y);
            position.y = f (xz, t);
            point.localPosition = position;
        }
    }

    static float SineFunction (Vector2 xz, float t) {
        float x = xz.x;
        return Mathf.Sin (pi * (x + t));
    }

    static float MultiSineFunction (Vector2 xz, float t) {
        float x = xz.x;
        float y = Mathf.Sin (pi * (x + t));
        // add another sine wave with half the freq
        // and amplitude
        y += Mathf.Sin (2f * pi * (x + t)) / 2f;
        // amplitude of resulting function is 3/2
        // normalize
        y *= 2f / 3f;
        return y;
    }
}