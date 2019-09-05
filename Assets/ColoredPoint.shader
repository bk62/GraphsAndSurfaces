﻿Shader "Custom/ColoredPoint"
{
    // Color based on position
    // Moving objects affects the color
    // Color determined per vertex and interpolated along faces
    // so transitions become more clear for large objects

    Properties
    {
        // _Color ("Color", Color) = (1,1,1,1)
        // _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // sampler2D _MainTex;

        struct Input
        {
            // float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        // fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            // o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables.r
            // red transition from x = -1 to 1
            // green transtion for y
            o.Albedo.rgb = IN.worldPos.xyz * 0.5 + 0.5;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1; // c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
