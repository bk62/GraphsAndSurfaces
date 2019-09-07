﻿using System.Collections;
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
        MultiSineFunction,
        DiagonalSine2DFunction,
        Sine2DFunction,
        MultiSine2DFunction,
        RippleFunction
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
            Vector2 xz = new Vector2 (position.x, position.z);
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

    static float DiagonalSine2DFunction(Vector2 xz, float t) {
        // diagonal sine wave
        float x = xz.x;
        float z = xz.y;
        return Mathf.Sin(pi * (x + z + t));
    }

    static float Sine2DFunction(Vector2 xz, float t) {
        // sum of wave along each dimension
        float x = xz.x;
        float z = xz.y;
        float y = Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(pi * (z + t));
        y *= 0.5f;
        return y;
    }

    static float MultiSine2DFunction(Vector2 xz, float t) {
        // single main wave with two secondary waves -- one per dimension
        // main wave is a slow moving diagonal wave
        // secondary waves move along the axes
        // main wave has half and z wave has double x waves freq
        float x = xz.x;
        float z = xz.y;
        float y = 4f * Mathf.Sin(pi * (x + z + t * 0.5f));
        y += Mathf.Sin(pi * (x + t));
        y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        y *= 1f / 5.5f;
        return y;
    }

    static float RippleFunction (Vector2 xz, float t) {
        float x = xz.x;
        float z = xz.y;
        float d = Mathf.Sqrt(x*x + z*z);
        // outward mogin high freq sine wave with distance from origin as input
        float y = Mathf.Sin(pi*(4f * d - t));
        // amplitude inverse reln to distance (adding 1 to avoid div by 0)
        y /= 1f + 10f * d;
        return y;
    }
}