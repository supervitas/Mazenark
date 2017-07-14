Shader "Custom/DisolveShaderUnlit" {
    Properties {
         _MainTex ("Texture (RGB)", 2D) = "white" {}
         _SliceGuide ("Slice Guide (RGB)", 2D) = "white" {}
         _SliceAmount ("Slice Amount", Range(0.0, 1.0)) = 0.0     
         _BurnSize ("Burn Size", Range(0.0, 1.0)) = 0.4
         _BurnRamp ("Burn Ramp (RGB)", 2D) = "white" {}
         _Color ("Diffuse", Color) = (1,1,1,1)
     }
	SubShader {
		Tags { "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
		LOD 300
		
		CGPROGRAM
		#pragma surface surf NoLighting  noforwardadd noambient vertex:vert
        #pragma target 3.0
        
        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
            fixed4 c;
            c.rgb = s.Albedo; 
            c.a = s.Alpha;
            return c;
        }       


		  sampler2D _MainTex;
          sampler2D _SliceGuide;
          float _SliceAmount;
          sampler2D _BurnRamp;
          float _BurnSize;
          fixed4 _Color;         


		struct Input {
		    float2 uv_MainTex;
            float2 uv_SliceGuide;            
		};
		
       float rand(float3 myVector)  {
            return frac(sin( dot(myVector ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
        }
		
        void vert (inout appdata_full v) {
        
           v.vertex.x -= (_SliceAmount * _SliceAmount);
           v.vertex.z -= (_SliceAmount * _BurnSize);  
             
        }

		 void surf (Input IN, inout SurfaceOutput o) {
             clip(tex2D (_SliceGuide, IN.uv_SliceGuide).rgb - _SliceAmount);
             o.Albedo = tex2D (_MainTex, IN.uv_MainTex) * _Color;
     
             half test = tex2D (_SliceGuide, IN.uv_MainTex).rgb - _SliceAmount;
             
             if(test < _BurnSize && _SliceAmount > 0 && _SliceAmount < 1) {
                o.Emission = tex2D(_BurnRamp, float2(test * (1 / _BurnSize), 0));
                o.Albedo *= o.Emission;
             }
         }
         
		ENDCG
	} 
	FallBack "Transparent/Cutout/VertexLit"
}