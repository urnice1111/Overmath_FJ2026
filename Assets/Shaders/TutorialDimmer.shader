Shader "UI/TutorialDimmer"
{
    Properties
    {
        _DimAlpha ("Dim Alpha", Range(0, 1)) = 0.6
        _HoleRect ("Hole Rect (xMin, yMin, xMax, yMax)", Vector) = (-1, -1, -1, -1)
        _HoleRadius ("Hole Corner Radius", Float) = 12

        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 color : COLOR;
            };

            float _DimAlpha;
            float4 _HoleRect;
            float _HoleRadius;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                o.color = v.color;
                return o;
            }

            float roundedRectSDF(float2 p, float2 center, float2 halfSize, float radius)
            {
                float2 d = abs(p - center) - halfSize + radius;
                return length(max(d, 0.0)) - radius;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 screen = i.screenPos.xy / i.screenPos.w;

                if (_HoleRect.x >= 0)
                {
                    float2 holeCenter = (_HoleRect.xy + _HoleRect.zw) * 0.5;
                    float2 holeHalf = (_HoleRect.zw - _HoleRect.xy) * 0.5;

                    float2 pixelSize = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);
                    float radiusNorm = _HoleRadius * min(pixelSize.x, pixelSize.y);

                    float dist = roundedRectSDF(screen, holeCenter, holeHalf, radiusNorm);

                    float edge = fwidth(dist) * 1.5;
                    float alpha = smoothstep(-edge, edge, dist);

                    return fixed4(0, 0, 0, _DimAlpha * alpha);
                }

                return fixed4(0, 0, 0, _DimAlpha);
            }
            ENDCG
        }
    }
}
