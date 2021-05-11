Shader "Custom/UIBlur"
{

    Properties
    {
        [Toggle(IS_BLUR_ALPHA_MASKED)] _IsAlphaMasked("Image Alpha Masks Blur", Float) = 1
        [Toggle(IS_SPRITE_VISIBLE)] _IsSpriteVisible("Show Image", Float) = 1

        // Internally enforced by MAX_RADIUS
        _Radius("Blur Radius", Range(0, 64)) = 1
        _OverlayColor("Blurred Overlay/Opacity", Color) = (0.5, 0.5, 0.5, 1)

        // see Stencil in UI/Default
        [HideInInspector][PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
        [HideInInspector]_UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    Category
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

        SubShader
        {

            GrabPass
            {
                Tags
                { 
                    "LightMode" = "Always"
                    "Queue" = "Background"  
                }
            }

            Pass
            {
                Name "UIBlur_Y"
                Tags{ "LightMode" = "Always" }

            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #pragma multi_compile __ IS_BLUR_ALPHA_MASKED
                #pragma multi_compile __ IS_SPRITE_VISIBLE
                #pragma multi_compile __ UNITY_UI_ALPHACLIP

                #include "UIBlur_Shared.cginc"

                sampler2D _GrabTexture;
                float4 _GrabTexture_TexelSize;

                half4 frag(v2f IN) : COLOR
                {
                    half4 pixel_raw = tex2D(_MainTex, IN.uvmain);
                    return GetBlurInDir(IN, pixel_raw, _GrabTexture, _GrabTexture_TexelSize, 0, 1);
                }
            ENDCG
            }

            
            GrabPass
            {
                Tags
                { 
                    "LightMode" = "Always"
                    "Queue" = "Background"  
                }
            }
            Pass
            {
                Name "UIBlur_X"
                Tags{ "LightMode" = "Always" }


            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #pragma multi_compile __ IS_BLUR_ALPHA_MASKED
                #pragma multi_compile __ IS_SPRITE_VISIBLE
                #pragma multi_compile __ UNITY_UI_ALPHACLIP

                #include "UIBlur_Shared.cginc"


                sampler2D _GrabTexture;
                float4 _GrabTexture_TexelSize;

                half4 frag(v2f IN) : COLOR
                {
                    half4 pixel_raw = tex2D(_MainTex, IN.uvmain);

#if IS_SPRITE_VISIBLE
                    return layerBlend(
                            // Layer 0 : The blurred background
                            GetBlurInDir(IN, pixel_raw, _GrabTexture, _GrabTexture_TexelSize, 1, 0), 
                            // Layer 1 : The sprite itself
                            pixel_raw * IN.color
                        );
#else
                    return GetBlurInDir(IN, pixel_raw, _GrabTexture, _GrabTexture_TexelSize, 1, 0);
#endif
                }
            ENDCG
            }

        }
    }
    Fallback "UI/Default"
}
