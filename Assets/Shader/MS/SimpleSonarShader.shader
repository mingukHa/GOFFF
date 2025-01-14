Shader "MadeByProfessorOakie/URP_SimpleSonarShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        //_RingColor("Ring Color", Color) = (1, 1, 1, 1) // 링 색상, Inspector에서 설정
        _RingColorIntensity("Ring Color Intensity", Float) = 2
        _RingSpeed("Ring Speed", Float) = 1
        _RingWidth("Ring Width", Float) = 0.1
        _RingIntensityScale("Ring Intensity Scale", Float) = 1
        _RingTex("Ring Texture", 2D) = "white" {}
        //_RingColor("Ring Color" Color) = (1, 1, 1, 1)
        _OutlineColor("OutlineColor", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Float) = 0.02
        _DistanceFactor("Distance Factor", Float) = 0.1
        _OutlineAlpha("Outline Alpha", Float) = 1.0

        _OutlineRingSpeed("Outline Ring Speed", Float) = 1
        _OutlineRingWitdh("Outline Ring Width", Float) = 0.1


        _RingFadeDuration("Ring Fade Duration", Float) = 2

        _Type("Type", int) = 0

        _RingTime("Ring Time", Float) = 0.1
    }

    SubShader
    {
        //Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
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
            float4 _RingColor[100];
            float _RingColorIntensity;
            float _RingSpeed;
            float _RingWidth;
            float _RingIntensityScale;
            float _RingFadeDuration;

            float _ringlength;

            float4 _hitPts[100];
            float _StartTime;
            float _Intensity[100];

            float4 _RingColorW = float4(1, 1, 1, 1);

            float _RingTime;


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
                     
                for (int idx = 0; idx < 100; idx++)
                {
                    float diffFromRingCol = abs(finalColor.r - _RingColor[idx].r) +
                                        abs(finalColor.g - _RingColor[idx].g) +
                                        abs(finalColor.b - _RingColor[idx].b);

                    float3 hitPos = _hitPts[idx].xyz;
                    float hitTime = _hitPts[idx].w;
                    float intensity = _Intensity[idx] * _RingIntensityScale;

                    float dist = distance(hitPos, i.worldPos);
                    float ringStart = (_RingTime - hitTime) * _RingSpeed - _RingWidth;
                    float ringEnd = (_RingTime - hitTime) * _RingSpeed;
                    // float ringStart = (_RingTime * _RingSpeed - _RingWidth) * sin(_Time.y * 0.5) + 1.0;  // 시간에 따라 크기 변화
                    // float ringEnd = (_RingTime * _RingSpeed) * sin(_Time.y * 0.5) + 1.0;  // 링 크기 변화를 계산

                    if (dist > ringStart && dist < ringEnd)
                    {
                        float ringProgress = (dist - ringStart) / _RingWidth;
                        float val = (1 - (dist / intensity)) *
                                    tex2D(_RingTex, float2(1 - ringProgress, 0.5)).r;

                        if (val > 0)
                        {
                            float3 ringColor = _RingColor[idx].rgb * val * _RingColorIntensity;
                            float3 blendedColor = lerp(finalColor, ringColor, val);

                            float newDiffFromRingCol = abs(blendedColor.r - _RingColor[idx].r) +
                                                        abs(blendedColor.g - _RingColor[idx].g) +
                                                        abs(blendedColor.b - _RingColor[idx].b);

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
                float3 worldPos : TEXCOORD0;
                float3 originalWorldPos : TEXCOORD1;
            };

            float _OutlineWidth;
            float4 _OutlineColor;
            float4 _RingColorM;
            float _RingSpeed; // 첫 번째 패스와 동일한 속도 사용
            float _RingWidth; // 첫 번째 패스와 동일한 폭 사용
            float4 _hitPtsM[100];
            float _StartTime;
            float _RingFadeDuration;
            float _OutlineAlpha;
            float _DistanceFactor;
            float _OutlinePower;

            v2f vert(appdata v)
            {
                v2f o;
                o.originalWorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 viewDir = normalize(_WorldSpaceCameraPos - o.originalWorldPos);
                float3 normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                float fresnel = 1.0 - saturate(dot(viewDir, normal));
                fresnel = pow(fresnel, _OutlinePower);
                float outlineScale = _OutlineWidth * (1.0 + _DistanceFactor * distance(o.originalWorldPos, _WorldSpaceCameraPos));
                v.vertex.xyz += v.normal * outlineScale;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _OutlineColor;
                col.a = _OutlineAlpha;

                bool isInAnyRing = false;
                float maxRingIntensity = 0;

                for (int idx = 0; idx < 100; idx++)
                {
                    float3 hitPos = _hitPtsM[idx].xyz;
                    float hitTime = _hitPtsM[idx].w;
                    
                    if (hitTime <= 0) continue; // 유효하지 않은 히트 포인트 스킵

                    float dist = distance(hitPos, i.originalWorldPos);
                    float timeSinceHit = _Time.y - hitTime;
                    
                    // 첫 번째 패스와 동일한 링 범위 계산
                    float ringPosition = timeSinceHit * _RingSpeed;
                    float ringStart = ringPosition - _RingWidth;
                    float ringEnd = ringPosition;

                    if (dist >= ringStart && dist <= ringEnd)
                    {
                        float ringProgress = (dist - ringStart) / _RingWidth;
                        float ringIntensity = 1 - ringProgress;
                        
                        // 페이드 아웃 효과
                        float fadeProgress = 1 - (timeSinceHit / _RingFadeDuration);
                        fadeProgress = saturate(fadeProgress);
                        
                        ringIntensity *= fadeProgress;
                        
                        if (ringIntensity > maxRingIntensity)
                        {
                            maxRingIntensity = ringIntensity;
                            isInAnyRing = true;
                        }
                    }
                }

                if (isInAnyRing)
                {
                    col.rgb = lerp(col.rgb, _RingColorM.rgb, maxRingIntensity);
                    col.a *= maxRingIntensity;
                }
                else
                {
                    col.a = 0; // 링 밖에서는 아웃라인을 표시하지 않음
                }

                if (col.a <= 0.0)
                {
                    discard;
                }

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDHLSL
        }


        // 아웃 라인 그리는 부분
        // Pass
        // {
        //     Name "OUTLINE"
        //     Cull Front
        //     ZWrite ON
        //     ZTest LEqual

        //     Blend SrcAlpha OneMinusSrcAlpha

        //     HLSLPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag
        //     #pragma multi_compile_fog

        //     #include "UnityCG.cginc"

        //     struct appdata
        //     {
        //         float4 vertex : POSITION;
        //         float3 normal : NORMAL;
        //     };

        //     struct v2f
        //     {
        //         UNITY_FOG_COORDS(1)
        //         float4 vertex : SV_POSITION;
        //         float3 worldPos : TEXCOORD0; // 월드 좌표 추가
        //         float3 originalWorldPos : TEXCOORD1; // 원래 위치의 월드 좌표 추가
        //     };

        //     float _OutlineWidth;
        //     float4 _OutlineColor;
        //     float4 _RingColor[100]; // 링 색상 추가
        //     float _OutlineRingSpeed;
        //     float _OutlineRingWidth;
        //     float4 _hitPts[100];
        //     float _StartTime;
        //     float _RingFadeDuration;
        //     float _OutlineAlpha;
        //     float _DistanceFactor; // 거리 기반 스케일링을 위한 팩터
        //     float _OutlinePower; // Fresnel 효과의 강도 조절

        //     int _Type;

        //     float _RingTime;

        //     v2f vert(appdata v)
        //     {
        //         v2f o;

        //         // 월드 좌표 계산
        //         o.originalWorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

        //         // 카메라 방향 계산
        //         float3 viewDir = normalize(_WorldSpaceCameraPos - o.originalWorldPos);
        //         // 법선 방향
        //         float3 normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

        //         // Fresnel 효과 계산 (내적값을 사용)
        //         float fresnel = 1.0 - saturate(dot(viewDir, normal));
        //         fresnel = pow(fresnel, _OutlinePower); // 강조 정도를 조절

        //         // 월드 공간에서 일정한 아웃라인 크기를 유지하도록 조정
        //         float outlineScale = _OutlineWidth * (1.0 + _DistanceFactor * distance(o.originalWorldPos, _WorldSpaceCameraPos));

        //         // 법선 방향으로 정점 이동 (아웃라인 생성)
        //         v.vertex.xyz += v.normal * outlineScale; // 아웃라인 크기를 일정하게 유지

        //         o.vertex = UnityObjectToClipPos(v.vertex);

        //         // 변위된 월드 좌표 계산
        //         o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

        //         UNITY_TRANSFER_FOG(o, o.vertex);
        //         return o;
        //     }

        //     fixed4 frag(v2f i) : SV_Target
        //     {
        //         fixed4 col = _OutlineColor;
        //         col.a = _OutlineAlpha;; // 기본 알파값 1

        //         float mostRecentTime = -1.0; // 가장 최근 파동의 시간
        //         float3 mostRecentPos = float3(0, 0, 0); // 가장 최근 파동의 위치

        //         for (int idx = 0; idx < 100; idx++)
        //         {

        //             float3 hitPos = _hitPts[idx].xyz;  // 파동의 위치
        //             float hitTime = _hitPts[idx].w;   // 파동의 시작 시간

        //             float dist = distance(hitPos, i.originalWorldPos);  // 현재 픽셀과 파동의 거리
        //             float ringStart = (_RingTime - hitTime) * _OutlineRingSpeed - _OutlineRingWidth;
        //             float ringEnd = (_RingTime - hitTime) * _OutlineRingSpeed;

        //             // 링이 이 픽셀을 지나간 경우
        //             if ( ringStart -0.1f && ringEnd > dist && hitTime > mostRecentTime)
        //             {
        //                 mostRecentTime = hitTime;   // 가장 최근 시간 업데이트
        //                 mostRecentPos = hitPos;    // 가장 최근 위치 업데이트
        //             }
        //                             // 가장 최근에 영향을 준 파동이 있을 경우 페이드 적용
        //         if (mostRecentTime > 0)
        //         {
        //             float fadeTime = _RingFadeDuration;
        //             float fadeProgress = 1 - ((_RingTime - mostRecentTime) / fadeTime);
        //             fadeProgress = saturate(fadeProgress); // 0~1로 제한
        //             //col = _RingColor; // 링 색상 적용
        //             float nonLinearFade = pow(fadeProgress, 0.6);
        //             col.a = nonLinearFade; // 알파값은 페이드 프로그래스
        //         }
        //         }

        //         if (col.a <= 0.0)
        //         {
        //             discard; // 아웃라인을 그리지 않음
        //         }

        //         UNITY_APPLY_FOG(i.fogCoord, col);
        //         return col;
        //     }
        //     ENDHLSL
        // }

    }

    FallBack "Diffuse"
}