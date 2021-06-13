Shader "PolygonArsenal/PolyRimLightTransparent"
 {
     Properties 
     {
       /*_InnerColor ("Inner Color", Color) = (1.0, 1.0, 1.0, 1.0)
       _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
       _RimWidth ("Rim Width", Range(0.2,20.0)) = 3.0
	   _RimGlow ("Rim Glow Multiplier", Range(0.0,9.0)) = 1.0*/
     }
     SubShader 
     {
       Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
       
       Cull Back
	   Lighting Off
       Blend One One
       
       CGPROGRAM
       #pragma surface surf Lambert
       
       struct Input 
       {
           float3 viewDir;
       };
       
	 /*CBUFFER_START(UnityPerDraw)
       float4 _InnerColor;
       float4 _RimColor;
       float _RimWidth;
	   float _RimGlow;
	   CBUFFER_END*/

	   
       void surf (Input IN, inout SurfaceOutput o) 
       {
		   /*float4 _InnerColor = float4(0, 0, 0, 1);
		   float4 _RimColor = float4(0, 0, 1, 1);
		   float _RimWidth = 7.4;
		   float _RimGlow = 3.41;
           o.Albedo = _InnerColor.rgb;
           half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
           o.Emission = _RimColor.rgb * _RimGlow * pow (rim, _RimWidth);*/

		   float4 innerColor = float4(0, 0, 0, 1);
		   float4 rimColor = float4(0, 0, 1, 1);
		   float rimWidth = 7.4;
		   float rimGlow = 3.41;
		   o.Albedo = innerColor.rgb;
		   half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		   o.Emission = rimColor.rgb * rimGlow * pow(rim, rimWidth);
       }
       ENDCG
     } 
     Fallback "Diffuse"
   }