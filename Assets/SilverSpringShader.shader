Shader "Custom/SilverSpringShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = i.uv.x;
                float3 silver = float3(0.75, 0.75, 0.75);
                float3 white = float3(1.0, 1.0, 1.0);

                float3 color = lerp(silver, white, smoothstep(0.0, 0.5, t));
                color = lerp(color, silver, smoothstep(0.5, 1.0, t));

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
