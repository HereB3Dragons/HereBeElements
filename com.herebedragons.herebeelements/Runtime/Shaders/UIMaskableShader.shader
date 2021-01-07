Shader "Unlit Master"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTexture", 2D) = "white" {}
        [HDR]_Color("TintColor", Color) = (1, 1, 1, 1)
        opacity("Opacity", Range(0, 1)) = 1
        alphaClip("AlphaClip", Range(0, 1)) = 0.5
        [ToggleUI]border("Border", Float) = 1
        borderWidth("BorderWidth", Range(0, 0.99)) = 0.5
        [HDR]borderColor("BorderColor", Color) = (1, 0, 0, 1)
        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent+0"
        }
        
        Pass
        {
            Name "Pass"
            Tags 
            { 
                // LightMode: <None>
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest [unity_GUIZTestMode]
            ZWrite Off
            // ColorMask: <None>
            
            Stencil{
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            ColorMask [_ColorMask]
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma shader_feature _ _SAMPLE_GI
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define SHADERPASS_UNLIT
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float opacity;
            float alphaClip;
            float border;
            float borderWidth;
            float4 borderColor;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_F1C658AE_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_InvertColors_float(float In, float InvertColors, out float Out)
            {
                Out = abs(InvertColors - In);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
            
            struct Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2
            {
                float4 VertexColor;
                half4 uv0;
            };
            
            void SG_BaseUISubGraph_a5fc881c741200946b26e65828a351c2(TEXTURE2D_PARAM(Texture2D_F3C635FC, samplerTexture2D_F3C635FC), float4 Texture2D_F3C635FC_TexelSize, float Vector1_5BC3683, float4 Vector4_15401CDB, Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2 IN, out float Alpha_1, out float4 Color_2)
            {
                float4 _SampleTexture2D_F1C658AE_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_F3C635FC, samplerTexture2D_F3C635FC, IN.uv0.xy);
                float _SampleTexture2D_F1C658AE_R_4 = _SampleTexture2D_F1C658AE_RGBA_0.r;
                float _SampleTexture2D_F1C658AE_G_5 = _SampleTexture2D_F1C658AE_RGBA_0.g;
                float _SampleTexture2D_F1C658AE_B_6 = _SampleTexture2D_F1C658AE_RGBA_0.b;
                float _SampleTexture2D_F1C658AE_A_7 = _SampleTexture2D_F1C658AE_RGBA_0.a;
                float4 _Property_40121039_Out_0 = Vector4_15401CDB;
                float4 _Multiply_70799BE2_Out_2;
                Unity_Multiply_float(_SampleTexture2D_F1C658AE_RGBA_0, _Property_40121039_Out_0, _Multiply_70799BE2_Out_2);
                float4 _Multiply_F89BFEEC_Out_2;
                Unity_Multiply_float(_Multiply_70799BE2_Out_2, IN.VertexColor, _Multiply_F89BFEEC_Out_2);
                float _Split_51753E22_R_1 = _Multiply_F89BFEEC_Out_2[0];
                float _Split_51753E22_G_2 = _Multiply_F89BFEEC_Out_2[1];
                float _Split_51753E22_B_3 = _Multiply_F89BFEEC_Out_2[2];
                float _Split_51753E22_A_4 = _Multiply_F89BFEEC_Out_2[3];
                float _InvertColors_DD4AEEA3_Out_1;
                float _InvertColors_DD4AEEA3_InvertColors = float (0
            );    Unity_InvertColors_float(_Split_51753E22_A_4, _InvertColors_DD4AEEA3_InvertColors, _InvertColors_DD4AEEA3_Out_1);
                float _InvertColors_D4D08E04_Out_1;
                float _InvertColors_D4D08E04_InvertColors = float (0
            );    Unity_InvertColors_float(_SampleTexture2D_F1C658AE_A_7, _InvertColors_D4D08E04_InvertColors, _InvertColors_D4D08E04_Out_1);
                float _Multiply_4E04F8CF_Out_2;
                Unity_Multiply_float(_InvertColors_DD4AEEA3_Out_1, _InvertColors_D4D08E04_Out_1, _Multiply_4E04F8CF_Out_2);
                float _Property_A33AE95E_Out_0 = Vector1_5BC3683;
                float _OneMinus_1FEB124A_Out_1;
                Unity_OneMinus_float(_Property_A33AE95E_Out_0, _OneMinus_1FEB124A_Out_1);
                float _Subtract_3DCDAD49_Out_2;
                Unity_Subtract_float(_Multiply_4E04F8CF_Out_2, _OneMinus_1FEB124A_Out_1, _Subtract_3DCDAD49_Out_2);
                Alpha_1 = _Subtract_3DCDAD49_Out_2;
                Color_2 = _Multiply_F89BFEEC_Out_2;
            }
            
            void Unity_Step_float(float Edge, float In, out float Out)
            {
                Out = step(Edge, In);
            }
            
            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }
            
            void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
            {
                Out = Predicate ? True : False;
            }
            
            struct Bindings_BorderUISubGraph_af915c8dab03b6a488f8232a31e0b50b
            {
            };
            
            void SG_BorderUISubGraph_af915c8dab03b6a488f8232a31e0b50b(float Boolean_C9DE3548, float4 Vector4_60D0DA82, float4 Vector4_1CDDC39F, float Vector1_5FEC0E3B, float Vector1_80F00D02, Bindings_BorderUISubGraph_af915c8dab03b6a488f8232a31e0b50b IN, out float4 OutVector4_1)
            {
                float _Property_6E34D0BF_Out_0 = Boolean_C9DE3548;
                float _Property_75821224_Out_0 = Vector1_5FEC0E3B;
                float _Property_C0A3D0C9_Out_0 = Vector1_80F00D02;
                float _OneMinus_F0B62C27_Out_1;
                Unity_OneMinus_float(_Property_C0A3D0C9_Out_0, _OneMinus_F0B62C27_Out_1);
                float _Step_F16C73E5_Out_2;
                Unity_Step_float(_Property_75821224_Out_0, _OneMinus_F0B62C27_Out_1, _Step_F16C73E5_Out_2);
                float _InvertColors_AEF7F7A0_Out_1;
                float _InvertColors_AEF7F7A0_InvertColors = float (1
            );    Unity_InvertColors_float(_Step_F16C73E5_Out_2, _InvertColors_AEF7F7A0_InvertColors, _InvertColors_AEF7F7A0_Out_1);
                float4 _Property_6F53E243_Out_0 = Vector4_1CDDC39F;
                float4 _Multiply_2A62E937_Out_2;
                Unity_Multiply_float((_InvertColors_AEF7F7A0_Out_1.xxxx), _Property_6F53E243_Out_0, _Multiply_2A62E937_Out_2);
                float4 _Property_A206CE0E_Out_0 = Vector4_60D0DA82;
                float4 _Add_2FCD3637_Out_2;
                Unity_Add_float4(_Multiply_2A62E937_Out_2, _Property_A206CE0E_Out_0, _Add_2FCD3637_Out_2);
                float4 _Branch_32E73C6C_Out_3;
                Unity_Branch_float4(_Property_6E34D0BF_Out_0, _Add_2FCD3637_Out_2, _Property_A206CE0E_Out_0, _Branch_32E73C6C_Out_3);
                OutVector4_1 = _Branch_32E73C6C_Out_3;
            }
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
                float4 VertexColor;
            };
            
            struct SurfaceDescription
            {
                float3 Color;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Property_D04ED36C_Out_0 = border;
                float _Property_F86529F3_Out_0 = opacity;
                float4 _Property_DA8336CB_Out_0 = _Color;
                Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2 _BaseUISubGraph_A2484069;
                _BaseUISubGraph_A2484069.VertexColor = IN.VertexColor;
                _BaseUISubGraph_A2484069.uv0 = IN.uv0;
                float _BaseUISubGraph_A2484069_Alpha_1;
                float4 _BaseUISubGraph_A2484069_Color_2;
                SG_BaseUISubGraph_a5fc881c741200946b26e65828a351c2(TEXTURE2D_ARGS(_MainTex, sampler_MainTex), _MainTex_TexelSize, _Property_F86529F3_Out_0, _Property_DA8336CB_Out_0, _BaseUISubGraph_A2484069, _BaseUISubGraph_A2484069_Alpha_1, _BaseUISubGraph_A2484069_Color_2);
                float4 _Property_A49186A3_Out_0 = borderColor;
                float _Property_CB95F54B_Out_0 = borderWidth;
                Bindings_BorderUISubGraph_af915c8dab03b6a488f8232a31e0b50b _BorderUISubGraph_7B7473BB;
                float4 _BorderUISubGraph_7B7473BB_OutVector4_1;
                SG_BorderUISubGraph_af915c8dab03b6a488f8232a31e0b50b(_Property_D04ED36C_Out_0, _BaseUISubGraph_A2484069_Color_2, _Property_A49186A3_Out_0, _BaseUISubGraph_A2484069_Alpha_1, _Property_CB95F54B_Out_0, _BorderUISubGraph_7B7473BB, _BorderUISubGraph_7B7473BB_OutVector4_1);
                float _Property_F3EF134A_Out_0 = alphaClip;
                surface.Color = (_BorderUISubGraph_7B7473BB_OutVector4_1.xyz);
                surface.Alpha = _BaseUISubGraph_A2484069_Alpha_1;
                surface.AlphaClipThreshold = _Property_F3EF134A_Out_0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                output.interp01.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                output.color = input.interp01.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
                output.uv0 =                         input.texCoord0;
                output.VertexColor =                 input.color;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define SHADERPASS_SHADOWCASTER
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float opacity;
            float alphaClip;
            float border;
            float borderWidth;
            float4 borderColor;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_F1C658AE_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_InvertColors_float(float In, float InvertColors, out float Out)
            {
                Out = abs(InvertColors - In);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
            
            struct Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2
            {
                float4 VertexColor;
                half4 uv0;
            };
            
            void SG_BaseUISubGraph_a5fc881c741200946b26e65828a351c2(TEXTURE2D_PARAM(Texture2D_F3C635FC, samplerTexture2D_F3C635FC), float4 Texture2D_F3C635FC_TexelSize, float Vector1_5BC3683, float4 Vector4_15401CDB, Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2 IN, out float Alpha_1, out float4 Color_2)
            {
                float4 _SampleTexture2D_F1C658AE_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_F3C635FC, samplerTexture2D_F3C635FC, IN.uv0.xy);
                float _SampleTexture2D_F1C658AE_R_4 = _SampleTexture2D_F1C658AE_RGBA_0.r;
                float _SampleTexture2D_F1C658AE_G_5 = _SampleTexture2D_F1C658AE_RGBA_0.g;
                float _SampleTexture2D_F1C658AE_B_6 = _SampleTexture2D_F1C658AE_RGBA_0.b;
                float _SampleTexture2D_F1C658AE_A_7 = _SampleTexture2D_F1C658AE_RGBA_0.a;
                float4 _Property_40121039_Out_0 = Vector4_15401CDB;
                float4 _Multiply_70799BE2_Out_2;
                Unity_Multiply_float(_SampleTexture2D_F1C658AE_RGBA_0, _Property_40121039_Out_0, _Multiply_70799BE2_Out_2);
                float4 _Multiply_F89BFEEC_Out_2;
                Unity_Multiply_float(_Multiply_70799BE2_Out_2, IN.VertexColor, _Multiply_F89BFEEC_Out_2);
                float _Split_51753E22_R_1 = _Multiply_F89BFEEC_Out_2[0];
                float _Split_51753E22_G_2 = _Multiply_F89BFEEC_Out_2[1];
                float _Split_51753E22_B_3 = _Multiply_F89BFEEC_Out_2[2];
                float _Split_51753E22_A_4 = _Multiply_F89BFEEC_Out_2[3];
                float _InvertColors_DD4AEEA3_Out_1;
                float _InvertColors_DD4AEEA3_InvertColors = float (0
            );    Unity_InvertColors_float(_Split_51753E22_A_4, _InvertColors_DD4AEEA3_InvertColors, _InvertColors_DD4AEEA3_Out_1);
                float _InvertColors_D4D08E04_Out_1;
                float _InvertColors_D4D08E04_InvertColors = float (0
            );    Unity_InvertColors_float(_SampleTexture2D_F1C658AE_A_7, _InvertColors_D4D08E04_InvertColors, _InvertColors_D4D08E04_Out_1);
                float _Multiply_4E04F8CF_Out_2;
                Unity_Multiply_float(_InvertColors_DD4AEEA3_Out_1, _InvertColors_D4D08E04_Out_1, _Multiply_4E04F8CF_Out_2);
                float _Property_A33AE95E_Out_0 = Vector1_5BC3683;
                float _OneMinus_1FEB124A_Out_1;
                Unity_OneMinus_float(_Property_A33AE95E_Out_0, _OneMinus_1FEB124A_Out_1);
                float _Subtract_3DCDAD49_Out_2;
                Unity_Subtract_float(_Multiply_4E04F8CF_Out_2, _OneMinus_1FEB124A_Out_1, _Subtract_3DCDAD49_Out_2);
                Alpha_1 = _Subtract_3DCDAD49_Out_2;
                Color_2 = _Multiply_F89BFEEC_Out_2;
            }
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
                float4 VertexColor;
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Property_F86529F3_Out_0 = opacity;
                float4 _Property_DA8336CB_Out_0 = _Color;
                Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2 _BaseUISubGraph_A2484069;
                _BaseUISubGraph_A2484069.VertexColor = IN.VertexColor;
                _BaseUISubGraph_A2484069.uv0 = IN.uv0;
                float _BaseUISubGraph_A2484069_Alpha_1;
                float4 _BaseUISubGraph_A2484069_Color_2;
                SG_BaseUISubGraph_a5fc881c741200946b26e65828a351c2(TEXTURE2D_ARGS(_MainTex, sampler_MainTex), _MainTex_TexelSize, _Property_F86529F3_Out_0, _Property_DA8336CB_Out_0, _BaseUISubGraph_A2484069, _BaseUISubGraph_A2484069_Alpha_1, _BaseUISubGraph_A2484069_Color_2);
                float _Property_F3EF134A_Out_0 = alphaClip;
                surface.Alpha = _BaseUISubGraph_A2484069_Alpha_1;
                surface.AlphaClipThreshold = _Property_F3EF134A_Out_0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                output.interp01.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                output.color = input.interp01.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
                output.uv0 =                         input.texCoord0;
                output.VertexColor =                 input.color;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define SHADERPASS_DEPTHONLY
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float opacity;
            float alphaClip;
            float border;
            float borderWidth;
            float4 borderColor;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_F1C658AE_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_InvertColors_float(float In, float InvertColors, out float Out)
            {
                Out = abs(InvertColors - In);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
            
            struct Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2
            {
                float4 VertexColor;
                half4 uv0;
            };
            
            void SG_BaseUISubGraph_a5fc881c741200946b26e65828a351c2(TEXTURE2D_PARAM(Texture2D_F3C635FC, samplerTexture2D_F3C635FC), float4 Texture2D_F3C635FC_TexelSize, float Vector1_5BC3683, float4 Vector4_15401CDB, Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2 IN, out float Alpha_1, out float4 Color_2)
            {
                float4 _SampleTexture2D_F1C658AE_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_F3C635FC, samplerTexture2D_F3C635FC, IN.uv0.xy);
                float _SampleTexture2D_F1C658AE_R_4 = _SampleTexture2D_F1C658AE_RGBA_0.r;
                float _SampleTexture2D_F1C658AE_G_5 = _SampleTexture2D_F1C658AE_RGBA_0.g;
                float _SampleTexture2D_F1C658AE_B_6 = _SampleTexture2D_F1C658AE_RGBA_0.b;
                float _SampleTexture2D_F1C658AE_A_7 = _SampleTexture2D_F1C658AE_RGBA_0.a;
                float4 _Property_40121039_Out_0 = Vector4_15401CDB;
                float4 _Multiply_70799BE2_Out_2;
                Unity_Multiply_float(_SampleTexture2D_F1C658AE_RGBA_0, _Property_40121039_Out_0, _Multiply_70799BE2_Out_2);
                float4 _Multiply_F89BFEEC_Out_2;
                Unity_Multiply_float(_Multiply_70799BE2_Out_2, IN.VertexColor, _Multiply_F89BFEEC_Out_2);
                float _Split_51753E22_R_1 = _Multiply_F89BFEEC_Out_2[0];
                float _Split_51753E22_G_2 = _Multiply_F89BFEEC_Out_2[1];
                float _Split_51753E22_B_3 = _Multiply_F89BFEEC_Out_2[2];
                float _Split_51753E22_A_4 = _Multiply_F89BFEEC_Out_2[3];
                float _InvertColors_DD4AEEA3_Out_1;
                float _InvertColors_DD4AEEA3_InvertColors = float (0
            );    Unity_InvertColors_float(_Split_51753E22_A_4, _InvertColors_DD4AEEA3_InvertColors, _InvertColors_DD4AEEA3_Out_1);
                float _InvertColors_D4D08E04_Out_1;
                float _InvertColors_D4D08E04_InvertColors = float (0
            );    Unity_InvertColors_float(_SampleTexture2D_F1C658AE_A_7, _InvertColors_D4D08E04_InvertColors, _InvertColors_D4D08E04_Out_1);
                float _Multiply_4E04F8CF_Out_2;
                Unity_Multiply_float(_InvertColors_DD4AEEA3_Out_1, _InvertColors_D4D08E04_Out_1, _Multiply_4E04F8CF_Out_2);
                float _Property_A33AE95E_Out_0 = Vector1_5BC3683;
                float _OneMinus_1FEB124A_Out_1;
                Unity_OneMinus_float(_Property_A33AE95E_Out_0, _OneMinus_1FEB124A_Out_1);
                float _Subtract_3DCDAD49_Out_2;
                Unity_Subtract_float(_Multiply_4E04F8CF_Out_2, _OneMinus_1FEB124A_Out_1, _Subtract_3DCDAD49_Out_2);
                Alpha_1 = _Subtract_3DCDAD49_Out_2;
                Color_2 = _Multiply_F89BFEEC_Out_2;
            }
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
                float4 VertexColor;
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _Property_F86529F3_Out_0 = opacity;
                float4 _Property_DA8336CB_Out_0 = _Color;
                Bindings_BaseUISubGraph_a5fc881c741200946b26e65828a351c2 _BaseUISubGraph_A2484069;
                _BaseUISubGraph_A2484069.VertexColor = IN.VertexColor;
                _BaseUISubGraph_A2484069.uv0 = IN.uv0;
                float _BaseUISubGraph_A2484069_Alpha_1;
                float4 _BaseUISubGraph_A2484069_Color_2;
                SG_BaseUISubGraph_a5fc881c741200946b26e65828a351c2(TEXTURE2D_ARGS(_MainTex, sampler_MainTex), _MainTex_TexelSize, _Property_F86529F3_Out_0, _Property_DA8336CB_Out_0, _BaseUISubGraph_A2484069, _BaseUISubGraph_A2484069_Alpha_1, _BaseUISubGraph_A2484069_Color_2);
                float _Property_F3EF134A_Out_0 = alphaClip;
                surface.Alpha = _BaseUISubGraph_A2484069_Alpha_1;
                surface.AlphaClipThreshold = _Property_F3EF134A_Out_0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                output.interp01.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                output.color = input.interp01.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
                output.uv0 =                         input.texCoord0;
                output.VertexColor =                 input.color;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
            ENDHLSL
        }
        
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
