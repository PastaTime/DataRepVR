// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/MeshShader" {
     Properties {
          _MainTex ("Texture", 2D) = "white" {} 
          _Alpha("Transparency", Range(0,1.0)) = 1.0
         _CenterHeight ("Center Height", Float) = 0.0
         _MaxVariance ("Maximum Variance", Float) = 3.0
         _HighColor ("High Color", Color) = (1.0, 1.0, 1.0, 1.0)
         _LowColor ("Low Color", Color) = (0.0, 0.0, 0.0, 1.0)
         _x1("Lower X", Float) = 0.0
         _x2("Upper X", Float) = 0.0
         _z1("Lower Z", Float) = 0.0
         _z2("Upper Z", Float) = 0.0
     }
     SubShader {
             Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
             Cull Off
             
             CGPROGRAM
             #pragma surface surf Lambert vertex:vert alpha
             #include <UnityCG.cginc>

             float _Alpha;
             float _CenterHeight;
             float _MaxVariance;
             float4 _HighColor;
             float4 _LowColor;
             sampler2D _MainTex;

             float _x1;
			 float _x2;
		     float _z1;
			 float _z2;
             
             struct Input{
                 float2 uv_MainTex;
                 float4 color : COLOR;
                 float3 pos_ws : TEXCOORD4;
             };

             // Helper Function
             float3 Hue(float H)
			 {
			     float R = abs(H * 6 - 3) - 1;
			     float G = 2 - abs(H * 6 - 2);
			     float B = 2 - abs(H * 6 - 4);
			     return saturate(float3(R,G,B));
			 }
			 
			 float3 HSVtoRGB(in float3 HSV)
			 {
			     return float3(((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z);
			 }

			 float3 RGBtoHSV(in float3 RGB)
			 {
			     float3 HSV = 0;
			     HSV.z = max(RGB.r, max(RGB.g, RGB.b));
			     float M = min(RGB.r, min(RGB.g, RGB.b));
			     float C = HSV.z - M;
			     if (C != 0)
			     {
			         HSV.y = C / HSV.z;
			         float3 Delta = (HSV.z - RGB) / C;
			         Delta.rgb -= Delta.brg;
			         Delta.rg += float2(2,4);
			         if (RGB.r >= HSV.z)
			             HSV.x = Delta.b;
			         else if (RGB.g >= HSV.z)
			             HSV.x = Delta.r;
			         else
			             HSV.x = Delta.g;
			         HSV.x = frac(HSV.x / 6);
			     }
			     return float3(HSV);
			 }

			 // Lerps between two RGB values thru the HSV colorspace.
			 float4 hsv_lerp(float3 rgb_start, float3 rgb_end, float fraction)
             {
             	float3 hsv_start = RGBtoHSV(rgb_start);
             	float3 hsv_end = RGBtoHSV(rgb_end);

             	float3 hsv_out = lerp(hsv_start, hsv_end, fraction);

             	return float4(HSVtoRGB(hsv_out), _Alpha);
             }

             void vert(inout appdata_full v, out Input o) {

	             UNITY_INITIALIZE_OUTPUT(Input,o);
				//compute object vertices position in world space
				o.pos_ws = mul(unity_ObjectToWorld, v.vertex).xyz;

                 // Convert to world position
                 float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                 float diff = v.vertex.y - _CenterHeight;
                 float cFactor = saturate(diff/(2*_MaxVariance) + 0.5);
                 
                 //lerp by factor
                 v.color = lerp(_LowColor, _HighColor, cFactor);
             }

             void surf(Input IN, inout SurfaceOutput o){
	            
	            // Use X Pos to cut out object

				if (IN.pos_ws.x < _x1 || _x2 < IN.pos_ws.x)
				{
					discard;
				}
		
				
				// Use Z Pos to cut out object

				if (IN.pos_ws.z < _z1 || _z2 < IN.pos_ws.z)
				{
					discard;
				}


                 o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color; 
                 o.Alpha = _Alpha;  
             }


             
             ENDCG
     }
     FallBack "Diffuse"
 }