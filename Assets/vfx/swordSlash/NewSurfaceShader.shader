Shader "Custom/SwordSlashEffect"
{
    Properties
    {
        _MainTex ("Slash Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _EdgeColor ("Edge Color", Color) = (1,0.5,0,1)
        _Speed ("Animation Speed", Range(0.1, 5)) = 1
        _EdgeSharpness ("Edge Sharpness", Range(1, 10)) = 3
        _Intensity ("Glow Intensity", Range(0, 5)) = 1.5
        _Distortion ("Distortion Amount", Range(0, 0.2)) = 0.05
        _AlphaClip ("Alpha Clip Threshold", Range(0, 1)) = 0.1
    }
    
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }
        
        Blend SrcAlpha One // Additive blending
        Cull Off
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 noiseUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };
            
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;
            float4 _Color;
            float4 _EdgeColor;
            float _Speed;
            float _EdgeSharpness;
            float _Intensity;
            float _Distortion;
            float _AlphaClip;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.color = v.color;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Animate the noise texture
                float2 noiseUV = i.noiseUV;
                noiseUV.x += _Time.y * _Speed * 0.2;
                float noise = tex2D(_NoiseTex, noiseUV).r;
                
                // Apply noise to the main texture coordinates (distortion)
                float2 distortedUV = i.uv;
                distortedUV.x += (noise - 0.5) * _Distortion;
                distortedUV.y += (noise - 0.5) * _Distortion * 0.5;
                
                // Sample main texture with distorted UVs
                fixed4 mainTex = tex2D(_MainTex, distortedUV);
                
                // Create edge effect
                float edge = pow(mainTex.r, _EdgeSharpness);
                
                // Combine colors - core and edge
                fixed4 col = lerp(_Color, _EdgeColor, edge);
                
                // Apply glow intensity
                col.rgb *= _Intensity;
                
                // Apply the vertex color and texture alpha
                col.a = mainTex.a * i.color.a;
                
                // Alpha clip for cleaner edges
                clip(col.a - _AlphaClip);
                
                return col;
            }
            ENDCG
        }
    }
    
    Fallback "Diffuse"
}