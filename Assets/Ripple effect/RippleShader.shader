Shader "Hidden/RippleEffect"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off 
        Cull Off
        Pass
        {
            Name "RippleEffectPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            SAMPLER(sampler_BlitTexture);

            #define MAX_RIPPLES 10

            float4 _RippleData[MAX_RIPPLES]; 
            float _RippleAspectRatio;
            
            #define RIPPLE_THICKNESS 0.1 

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;

                float2 aspectUV = uv;
                aspectUV.x *= _RippleAspectRatio;
                

                float2 totalDistortion = float2(0, 0);


                for(int i = 0; i < MAX_RIPPLES; i++)
                {

                    float strength = _RippleData[i].w;
                    if (strength <= 0.0) continue; 

                    // Extract the packed data
                    float2 center = _RippleData[i].xy;
                    float progress = _RippleData[i].z;

                    float2 aspectCenter = center;
                    aspectCenter.x *= _RippleAspectRatio;

                    // Calculate Ring
                    float dist = distance(aspectUV, aspectCenter);
                    float ring = abs(dist - progress);
                    ring = 1.0 - smoothstep(0.0, RIPPLE_THICKNESS, ring);

                    float2 dir = normalize(uv - center + float2(0.0001, 0.0001));

                    totalDistortion += (dir * ring * strength);
                }

                float2 distortedUV = uv + totalDistortion;

                return SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, distortedUV);
            }
            ENDHLSL
        }
    }
}
