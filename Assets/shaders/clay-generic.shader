Shader "Custom/clay-generic"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

    }
    SubShader
    {


        Stencil {
             Ref 1         //0-255
             Comp Always     //default:always
            ZFail replace
        }


        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows  vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)



        float3 random3(float3 c) {
            float j = 4096.0 * sin(dot(c, float3(17.0, 59.4, 15.0)));
            float3 r;
            r.z = frac(512.0 * j);
            j *= .125;
            r.x = frac(512.0 * j);
            j *= .125;
            r.y = frac(512.0 * j);
            return r - 0.5;
        }

        /* skew constants for 3d simplex functions */
        const float F3 = 0.3333333;
        const float G3 = 0.1666667;

        float simplex3d(float3 p) {
            /* 1. find current tetrahedron T and it's four vertices */
            /* s, s+i1, s+i2, s+1.0 - absolute skewed (integer) coordinates of T vertices */
            /* x, x1, x2, x3 - unskewed coordinates of p relative to each of T vertices*/

            /* calculate s and x */
            float3 s = floor(p + dot(p, float3(F3, F3, F3)));
            float3 x = p - s + dot(s, float3(G3, G3, G3));

            /* calculate i1 and i2 */
            float3 e = step(float3(0.0, 0.0, 0.0), x - x.yzx);
            float3 i1 = e * (1.0 - e.zxy);
            float3 i2 = 1.0 - e.zxy * (1.0 - e);

            /* x1, x2, x3 */
            float3 x1 = x - i1 + G3;
            float3 x2 = x - i2 + 2.0 * G3;
            float3 x3 = x - 1.0 + 3.0 * G3;

            /* 2. find four surflets and store them in d */
            float4 w, d;

            /* calculate surflet weights */
            w.x = dot(x, x);
            w.y = dot(x1, x1);
            w.z = dot(x2, x2);
            w.w = dot(x3, x3);

            /* w fades from 0.6 at the center of the surflet to 0.0 at the margin */
            w = max(0.6 - w, 0.0);

            /* calculate surflet components */
            d.x = dot(random3(s), x);
            d.y = dot(random3(s + i1), x1);
            d.z = dot(random3(s + i2), x2);
            d.w = dot(random3(s + 1.0), x3);

            /* multiply d by w^4 */
            w *= w;
            w *= w;
            d *= w;

            /* 3. return the sum of the four surflets */
            return dot(d, float4(52.0, 52.0, 52.0, 52.0));
        }


        void vert(inout appdata_full v) {
            float fraction = 1.0 / 15.0;
            v.vertex.xyz += v.normal * simplex3d(v.vertex+floor(_Time.y*1000/ fraction)* fraction)*.05;
        }


        void surf (Input IN, inout SurfaceOutputStandard o)
        {


            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color ;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
