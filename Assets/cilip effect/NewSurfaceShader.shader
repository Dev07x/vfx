Shader "Custom/SimpleClipping"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ClipCenter ("Clip Center", Vector) = (0,0,0,0)
        _ClipSize ("Clip Size", Vector) = (1,1,1,0)
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
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
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 normal : TEXCOORD2;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float4 _ClipCenter;
            float4 _ClipSize;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate distance from clip center
                float3 clipPos = i.worldPos - _ClipCenter.xyz;
                
                // Check if inside clipping box
                if (abs(clipPos.x) < _ClipSize.x * 0.5 &&
                    abs(clipPos.y) < _ClipSize.y * 0.5 &&
                    abs(clipPos.z) < _ClipSize.z * 0.5)
                {
                    discard; // Remove this pixel
                }
                
                // Basic texture and color
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // Simple lighting
                float3 lightDir = normalize(float3(1, 1, 1));
                float NdotL = max(0, dot(i.normal, lightDir));
                col.rgb *= NdotL * 0.8 + 0.2; // Ambient + diffuse
                
                return col;
            }
            ENDCG
        }
    }
}