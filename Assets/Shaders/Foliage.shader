Shader "Sprites/Foliage"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _HorizontalShear ("Horizontal Shear", Float) = 0
        _VerticalShear ("Vetical Shear", Float) = 0

                _Speed ("Grass Speed", Range(0, 50)) = 1
        _Frequency ("Grass Frequncy", Range(0, 1)) = 0.1
        _Amplitude ("Grass Amplitude", Range(0, 1)) = 0.1

            }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "DisableBatching"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex MySpriteVert
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            float _HorizontalShear;
            float _VerticalShear;

            float _Speed;
            float _Frequency;
            float _Amplitude;

            float4 wind(float4 vertex, float2 uv)
            {
                vertex.x += sin((uv - (_Time.y * _Speed)) * _Frequency) * (uv.y * _Amplitude);
                float4 res = vertex;
                return res;
            }

            float4 applyShear(float4 vertex)
            {
                float4x4 shearTransformationMatrix = float4x4(
                    1, _HorizontalShear, 0, 0,
                    _VerticalShear, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                );

                float4 shearedVertex = mul(shearTransformationMatrix, vertex);
                return shearedVertex;
            }

            
            v2f MySpriteVert(appdata_t IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                //float4 shearedVertex = applyShear(IN.vertex);
                float4 res = wind(IN.vertex, IN.texcoord);


                OUT.vertex = UnityFlipSprite(res, _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                //OUT.vertex = mul(unity_MatrixMVP, shearedVertex);
                #ifdef PIXELSNAP_ON
    OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }
            ENDCG
        }

    }
}