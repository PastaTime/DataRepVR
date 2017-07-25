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
	            
	            //X pos & size will cutout the object

				if (IN.pos_ws.x < _x1 || _x2 < IN.pos_ws.x)
				{
					discard;
				}
		
				
				//Z pos & size will cutout the object

				if (IN.pos_ws.z < _z1 || _x2 < IN.pos_ws.z)
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