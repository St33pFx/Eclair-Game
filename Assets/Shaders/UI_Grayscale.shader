Shader "UI/Grayscale"
{
    Properties{
        _MainTex ("Sprite", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Saturation ("Saturation", Range(0,1)) = 0
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader{
        Tags{ "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
        Stencil{ Ref [_Stencil] Comp [_StencilComp] Pass [_StencilOp] ReadMask [_StencilReadMask] WriteMask [_StencilWriteMask] }
        Cull Off Lighting Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha ColorMask [_ColorMask]

        Pass{
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t { float4 vertex:POSITION; float2 texcoord:TEXCOORD0; float4 color:COLOR; };
            struct v2f { float4 vertex:SV_POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; };

            sampler2D _MainTex; float4 _MainTex_ST;
            fixed4 _Color; float _Saturation; float4 _ClipRect;

            v2f vert (appdata_t v){
                v2f o; o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
                o.color = v.color * _Color; return o;
            }

            fixed4 frag (v2f i):SV_Target{
                fixed4 c = tex2D(_MainTex, i.uv) * i.color;
                float g = dot(c.rgb, float3(0.299,0.587,0.114));
                c.rgb = lerp(float3(g,g,g), c.rgb, _Saturation);
                #ifdef UNITY_UI_CLIP_RECT
                    c.a *= UnityGet2DClipping(i.vertex, _ClipRect);
                #endif
                #ifdef UNITY_UI_ALPHACLIP
                    if (c.a < 0.001) discard;
                #endif
                return c;
            }
            ENDCG
        }
    }
}
