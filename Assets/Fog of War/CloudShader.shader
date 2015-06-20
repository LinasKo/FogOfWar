Shader "Custom/CloudShader"
{
    Properties
    {
        _MainTex ("Cloud Texture", 2D) = "white" {}
        _MinAlpha ("Minimum Opacity", Range(0,0.5)) = 0.1
        _MaxAlpha ("Maximum Opacity", Range(0.5,1)) = 0.98
    }

    SubShader {
        Tags { "RenderType"="Transparent" }
        Tags { "Queue"="Transparent" }
        LOD 200
        
        CGPROGRAM
        #pragma surface DrawClouds Lambert alpha noforwardadd

        sampler2D _MainTex;
        float _MinAlpha;
        float _MaxAlpha;

        sampler2D CurrentBeaconMap;
        float4 CurrentViewport;
        float4 AdjustViewpoint;

        sampler2D PreviousBeaconMap;
        float4 PreviousViewport;
        float PreviousPercent;

        float RollLeft1;
        float RollUp1;
        float RollLeft2;
        float RollUp2;
        float CosFactor;
        float Strength;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void DrawClouds (Input IN, inout SurfaceOutput o) {

            /*
             * Sample the current BeaconMap to calculate a cloud opacity.
             * Viewport vectors are x={Left} y={Top} z={Right} w={Bottom}
             */

            half opacity = Strength;
            half world_x = IN.worldPos.x + AdjustViewpoint.x;
            half world_z = IN.worldPos.z + AdjustViewpoint.z;

            if (CurrentViewport.x != CurrentViewport.z) {
                half uu = smoothstep(CurrentViewport.x, CurrentViewport.z, world_x);
                half vv = smoothstep(CurrentViewport.y, CurrentViewport.w, world_z);
                half4 got = tex2D (CurrentBeaconMap, half2(uu, vv));
                opacity = got.a*got.a;
            }

            if (PreviousPercent != 0) {
                half uu = smoothstep(PreviousViewport.x, PreviousViewport.z, world_x);
                half vv = smoothstep(PreviousViewport.y, PreviousViewport.w, world_z);
                half4 got = tex2D (PreviousBeaconMap, half2(uu, vv));
                half previousStrength = got.a*got.a;
                opacity = lerp(opacity, previousStrength, PreviousPercent);
            }

            /*
             * Now draw the clouds
             */

            half4 c1 = tex2D (_MainTex, half2(IN.uv_MainTex.x + RollLeft1, IN.uv_MainTex.y - RollUp1));
            half4 c2 = tex2D (_MainTex, 1 - half2(IN.uv_MainTex.x + RollLeft2, IN.uv_MainTex.y - RollUp2));
            half4 clr = lerp(c1, c2, CosFactor);
            o.Albedo = clr.rgb;

            half tt = lerp(_MinAlpha, _MaxAlpha, opacity);
            o.Alpha = clr.a * tt;

        }
        ENDCG
    } 
    FallBack "Diffuse"
}
