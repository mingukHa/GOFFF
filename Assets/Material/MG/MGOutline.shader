Shader "MadeByProfessorOakie/MG"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RingColor("Ring Color", Color) = (1, 1, 1, 1)
        _RingColorIntensity("Ring Color Intensity", Float) = 2
        _RingSpeed("Ring Speed", Float) = 1
        _RingWidth("Ring Width", Float) = 0.1
        _RingIntensityScale("Ring Intensity Scale", Float) = 1
        _RingTex("Ring Texture", 2D) = "white" {}
        _OutlineColor("OutlineColor", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", float) = 0.02
        _DistanceFactor("Distance Factor", Float) = 0.1
        _OutlineAlpha("Outline Alpha", Float) = 1.0
        _OutlineRingSpeed("Outline Ring Speed", Float) = 1
        _OutlineRingWidth("Outline Ring Width", Float) = 0.1
        _RingFadeDuration("Ring Fade Duration", Float) = 2
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            Cull Off
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // Unity �⺻ ���̺귯��

            sampler2D _MainTex;
            sampler2D _RingTex;

            float4 _BaseColor;
            float4 _RingColor;
            float _RingColorIntensity;
            float _RingSpeed;
            float _RingWidth;
            float _RingIntensityScale;
            float _RingFadeDuration;

            float4 _hitPts[100];
            float _StartTime;
            float _Intensity[100];

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = UnityObjectToClipPos(v.positionOS); // ��ü -> Ŭ�� ���� ��ȯ
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.positionOS).xyz; // ���� ��ǥ ���
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                float4 baseColor = tex2D(_MainTex, i.uv) * _BaseColor;
                float3 finalColor = baseColor.rgb;

                float diffFromRingCol = abs(finalColor.r - _RingColor.r) +
                                        abs(finalColor.g - _RingColor.g) +
                                        abs(finalColor.b - _RingColor.b);

                for (int idx = 0; idx < 100; idx++)
                {
                    float3 hitPos = _hitPts[idx].xyz;
                    float hitTime = _hitPts[idx].w;
                    float intensity = _Intensity[idx] * _RingIntensityScale;

                    float dist = distance(hitPos, i.worldPos);
                    float ringStart = (_Time.y - hitTime) * _RingSpeed - _RingWidth;
                    float ringEnd = (_Time.y - hitTime) * _RingSpeed;

                    if (dist > ringStart && dist < ringEnd)
                    {
                        float ringProgress = (dist - ringStart) / _RingWidth;
                        float val = (1 - (dist / intensity)) *
                                    tex2D(_RingTex, float2(1 - ringProgress, 0.5)).r;

                        if (val > 0)
                        {
                            float3 ringColor = _RingColor.rgb * val * _RingColorIntensity;
                            float3 blendedColor = lerp(finalColor, ringColor, val);

                            float newDiffFromRingCol = abs(blendedColor.r - _RingColor.r) +
                                                        abs(blendedColor.g - _RingColor.g) +
                                                        abs(blendedColor.b - _RingColor.b);

                            if (newDiffFromRingCol < diffFromRingCol)
                            {
                                finalColor = blendedColor;
                                diffFromRingCol = newDiffFromRingCol;
                            }
                        }
                    }
                }

                return float4(finalColor, baseColor.a);
            }
            ENDHLSL
        }

        Pass
        {
            Name "OUTLINE"
            Cull Off
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 originalWorldPos : TEXCOORD1;
            };

            float _OutlineWidth;
            float4 _OutlineColor;
            float _OutlineRingSpeed;
            float _OutlineRingWidth;
            float _OutlineAlpha;

            v2f vert(appdata v)
            {
                v2f o;
                o.originalWorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                float3 normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _OutlineColor;
                col.a = _OutlineAlpha;

                if (col.a <= 0.0)
                {
                    discard;
                }

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Diffuse"
}
