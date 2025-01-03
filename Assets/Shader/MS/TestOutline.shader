Shader "MadeByProfessorOakie/TestOutline"
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
        _OutlineColor("OutlineColor", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Float) = 0.02
        _DistanceFactor("Distance Factor", Float) = 0.1
        _OutlineAlpha("Outline Alpha", Float) = 1.0

        _OutlineRingSpeed("Outline Ring Speed", Float) = 1
        _OutlineRingWitdh("Outline Ring Width", Float) = 0.1


        _RingFadeDuration("Ring Fade Duration", Float) = 2

        _Type("Type", int) = 0
    }

    SubShader
    {
        //Tags { "RenderType" = "Opaque" "Queue" = "Opaque" }
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
                    float diffFromRingCol = abs(finalColor.r - _RingColor[idx].r) +
                                        abs(finalColor.g - _RingColor[idx].g) +
                                        abs(finalColor.b - _RingColor[idx].b);

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

        // 아웃 라인 그리는 부분
Pass
{
    Name "OUTLINE"
    Cull Front
    ZWrite Off
    ZTest LEqual
    Blend SrcAlpha OneMinusSrcAlpha

    HLSLPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile_fog

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    struct Attributes
    {
        float3 positionOS : POSITION; // Object Space 좌표
    };

    struct Varyings
    {
        float4 positionHCS : SV_POSITION; // Homogeneous Clip Space
        float2 uv : TEXCOORD0;            // 화면 공간 좌표
    };

    float _OutlineWidth;
    float4 _OutlineColor;
    float _OutlineAlpha;

    Varyings vert(Attributes v)
    {
        Varyings o;
        o.positionHCS = TransformObjectToHClip(v.positionOS);

        // 화면 공간 UV 좌표를 계산
        o.uv = (o.positionHCS.xy / o.positionHCS.w) * 0.5 + 0.5; // Normalize to [0, 1]
        return o;
    }

    // Declare the _CameraDepthTexture
    TEXTURE2D(_CameraDepthTexture);
    SAMPLER(sampler_CameraDepthTexture);

    float SampleDepth(float2 uv)
    {
        // Depth Texture 샘플링
        return SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;
    }

    float4 frag(Varyings i) : SV_Target
    {
        // 현재 픽셀의 깊이 값
        float currentDepth = SampleDepth(i.uv);
        
        // 주변 픽셀 깊이 값 샘플링
        float2 texelSize = _ScreenParams.zw * _OutlineWidth; // 텍셀 크기 계산
        float depthLeft = SampleDepth(i.uv + float2(-texelSize.x, 0));
        float depthRight = SampleDepth(i.uv + float2(texelSize.x, 0));
        float depthUp = SampleDepth(i.uv + float2(0, texelSize.y));
        float depthDown = SampleDepth(i.uv + float2(0, -texelSize.y));

        // 깊이 차이를 계산하여 에지를 감지
        float depthDiff = abs(currentDepth - depthLeft) +
                          abs(currentDepth - depthRight) +
                          abs(currentDepth - depthUp) +
                          abs(currentDepth - depthDown);

        // 에지 감지 기준 값
        float edgeThreshold = 0.1; // 이 값은 아웃라인 강도를 조절

        if (depthDiff > edgeThreshold)
        {
            float4 col = _OutlineColor;
            col.a = _OutlineAlpha; // 아웃라인 투명도
            return col;
        }

        // 아웃라인이 아닌 경우 투명 처리
        return float4(0, 0, 0, 0);
    }
    ENDHLSL
}




    }

    FallBack "Diffuse"
}