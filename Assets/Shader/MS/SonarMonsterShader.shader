Shader "MadeByProfessorOakie/URP_MonsterSonar"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        //_RingColor("Ring Color", Color) = (1, 1, 1, 1) // �� ����, Inspector���� ����
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
        _OutlineRingWitdh("Outline Ring Width", Float) = 0.1


        _RingFadeDuration("Ring Fade Duration", Float) = 2
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;
            sampler2D _RingTex;

            float4 _BaseColor;
            float4 _RingColorM;
            float _RingColorIntensity;
            float _RingSpeed;
            float _RingWidth;
            float _RingIntensityScale;
            float _RingFadeDuration;

            float4 _hitPtsM[100];
            float _StartTime;
            float _IntensityM[100];

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
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                o.worldPos = TransformObjectToWorld(v.positionOS).xyz;
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                float4 baseColor = tex2D(_MainTex, i.uv) * _BaseColor;
                float3 finalColor = baseColor.rgb;

                // float diffFromRingCol = abs(finalColor.r - _RingColor.r) +
                //                         abs(finalColor.g - _RingColor.g) +
                //                         abs(finalColor.b - _RingColor.b);

                for (int idx = 0; idx < 100; idx++)
                {
                    float diffFromRingCol = abs(finalColor.r - _RingColorM.r) +
                                        abs(finalColor.g - _RingColorM.g) +
                                        abs(finalColor.b - _RingColorM.b);

                    float3 hitPos = _hitPtsM[idx].xyz;
                    float hitTime = _hitPtsM[idx].w;
                    float intensity = _IntensityM[idx] * _RingIntensityScale;

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
                            float3 ringColor = _RingColorM.rgb * val * _RingColorIntensity;
                            float3 blendedColor = lerp(finalColor, ringColor, val);

                            float newDiffFromRingCol = abs(blendedColor.r - _RingColorM.r) +
                                                        abs(blendedColor.g - _RingColorM.g) +
                                                        abs(blendedColor.b - _RingColorM.b);

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

        // �ƿ� ���� �׸��� �κ�
        Pass
        {
            Name "OUTLINE"
            Cull Front
            ZWrite ON
            ZTest LEqual

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

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
                float3 worldPos : TEXCOORD0; // ���� ��ǥ �߰�
                float3 originalWorldPos : TEXCOORD1; // ���� ��ġ�� ���� ��ǥ �߰�
            };

            float _OutlineWidth;
            float4 _OutlineColor;
            float4 _RingColorM; // �� ���� �߰�
            float _OutlineRingSpeed;
            float _OutlineRingWidth;
            float4 _hitPtsM[100];
            float _StartTime;
            float _RingFadeDuration;
            float _OutlineAlpha;
            float _DistanceFactor; // �Ÿ� ��� �����ϸ��� ���� ����
            float _OutlinePower; // Fresnel ȿ���� ���� ����



            v2f vert(appdata v)
            {
                v2f o;

                // ���� ��ǥ ���
                o.originalWorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // ī�޶� ���� ���
                float3 viewDir = normalize(_WorldSpaceCameraPos - o.originalWorldPos);
                // ���� ����
                float3 normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

                // Fresnel ȿ�� ��� (�������� ���)
                float fresnel = 1.0 - saturate(dot(viewDir, normal));
                fresnel = pow(fresnel, _OutlinePower); // ���� ������ ����

                // ���� �������� ������ �ƿ����� ũ�⸦ �����ϵ��� ����
                float outlineScale = _OutlineWidth * (1.0 + _DistanceFactor * distance(o.originalWorldPos, _WorldSpaceCameraPos));

                // ���� �������� ���� �̵� (�ƿ����� ����)
                v.vertex.xyz += v.normal * outlineScale; // �ƿ����� ũ�⸦ �����ϰ� ����

                o.vertex = UnityObjectToClipPos(v.vertex);

                // ������ ���� ��ǥ ���
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _OutlineColor;
                col.a = _OutlineAlpha;; // �⺻ ���İ� 1



                float mostRecentTime = -1.0; // ���� �ֱ� �ĵ��� �ð�
                float3 mostRecentPos = float3(0, 0, 0); // ���� �ֱ� �ĵ��� ��ġ

                for (int idx = 0; idx < 100; idx++)
                {


                    float3 hitPos = _hitPtsM[idx].xyz;  // �ĵ��� ��ġ
                    float hitTime = _hitPtsM[idx].w;   // �ĵ��� ���� �ð�

                    float dist = distance(hitPos, i.originalWorldPos);  // ���� �ȼ��� �ĵ��� �Ÿ�
                    float ringStart = (_Time.y - hitTime) * _OutlineRingSpeed - _OutlineRingWidth;
                    float ringEnd = (_Time.y - hitTime) * _OutlineRingSpeed;

                    // ���� �� �ȼ��� ������ ���
                    if ( ringStart -0.01f&&ringEnd > dist && hitTime > mostRecentTime)
                    {
                        mostRecentTime = hitTime;   // ���� �ֱ� �ð� ������Ʈ
                        mostRecentPos = hitPos;    // ���� �ֱ� ��ġ ������Ʈ
                    }
                                    // ���� �ֱٿ� ������ �� �ĵ��� ���� ��� ���̵� ����
                if (mostRecentTime > 0)
                {
                    float fadeTime = _RingFadeDuration;
                    float fadeProgress = 1 - ((_Time.y - mostRecentTime) / fadeTime);
                    fadeProgress = saturate(fadeProgress); // 0~1�� ����
                    col.rgb = _RingColorM; // �� ���� ����
                    float nonLinearFade = pow(fadeProgress, 0.6);
                    col.a = nonLinearFade; // ���İ��� ���̵� ���α׷���
                }
                }



                if (col.a <= 0.0)
                {
                    discard; // �ƿ������� �׸��� ����
                }

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Diffuse"
}