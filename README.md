# HereBeElements
#### by HereBeDragons

## Description

### Miscellaneous

#### Detect CanvasGroup Interactions
https://forum.unity.com/threads/check-if-object-is-modified-by-canvasgroup.744149/

#### How to create maskable shaders
1. Open your UI Shader in ShaderGraph
2. Right click the shader node (the end one) and select `compile and view code`
3. copy the part the Stencil and ColorMask parts from the snippet below - _Position is Important!_
```shaderlab
Shader "Custom/Opaque"
{  
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

         // required for UI.Mask
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
         _ColorMask ("Color Mask", Float) = 15
         // ----
     }
     SubShader
     {
         Tags 
         { 
             // ...
         }
         
         // required for UI.Mask
         Stencil
         {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp] 
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
         }
          ColorMask [_ColorMask]
          // ----
         
         Pass
         {
             // ...
         }
     }
}

```

## Contributors
- Tiago Venceslau -  [Github](http://github.com/TiagoVenceslau)