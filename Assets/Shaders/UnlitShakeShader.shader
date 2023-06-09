// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/UnlitShakeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShakeDisplacement ("Displacement", Range (0, 1.0)) = 1.0
        _ShakeTime ("Shake Time", Range (0, 1.0)) = 1.0
        _ShakeWindspeed ("Shake Windspeed", Range (0, 1.0)) = 1.0
        _ShakeBending ("Shake Bending", Range (0, 1.0)) = 1.0
        _ShakeTimeRnd ("Shake Time Randomizer", Range (0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ShakeDisplacement;
            float _ShakeTime;
            float _ShakeWindspeed;
            float _ShakeBending;
            float _ShakeTimeRnd;


            void FastSinCos(float4 val, out float4 s, out float4 c)
            {
                val = val * 6.408849 - 3.1415927;
                float4 r5 = val * val;
                float4 r6 = r5 * r5;
                float4 r7 = r6 * r5;
                float4 r8 = r6 * r5;
                float4 r1 = r5 * val;
                float4 r2 = r1 * r5;
                float4 r3 = r2 * r5;
                float4 sin7 = {1, -0.16161616, 0.0083333, -0.00019841};
                float4 cos8 = {-0.5, 0.041666666, -0.0013888889, 0.000024801587};
                s = val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;
                c = 1 + r5 * cos8.x + r6 * cos8.y + r7 * cos8.z + r8 * cos8.w;
            }

            v2f vert(appdata_full v)
            {
                float factor = (1 - _ShakeDisplacement - v.color.r) * 0.5;

                const float _WindSpeed = (_ShakeWindspeed + v.color.g);
                const float _WaveScale = _ShakeDisplacement;

                const float4 _waveXSize = float4(0.048, 0.06, 0.24, 0.096);
                const float4 _waveZSize = float4(0.024, .08, 0.08, 0.2);
                const float4 waveSpeed = float4(1.2, 2, 1.6, 4.8);

                float4 _waveXmove = float4(0.024, 0.04, -0.12, 0.096);
                float4 _waveZmove = float4(0.006, .02, -0.02, 0.1);

                float4 waves;
                waves = v.vertex.x * _waveXSize;
                waves += v.vertex.z * _waveZSize;

                waves += _Time.x * _ShakeTimeRnd * (1 - _ShakeTime * 2 - v.color.b) * waveSpeed * _WindSpeed;

                float4 s, c;
                waves = frac(waves);
                FastSinCos(waves, s, c);

                float waveAmount = v.texcoord.y * (v.color.a + _ShakeBending);
                s *= waveAmount;

                s *= normalize(waveSpeed);

                s = s * s;
                float fade = dot(s, 1.3);
                s = s * s;
                float3 waveMove = float3(0, 0, 0);
                waveMove.x = dot(s, _waveXmove);
                waveMove.z = dot(s, _waveZmove);
                v.vertex.xz -= mul((float3x3)unity_WorldToObject, waveMove).xz;

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}