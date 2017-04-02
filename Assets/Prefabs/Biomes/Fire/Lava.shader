Shader "MazeBiomes/Lava"{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Diffuse", Color) = (1,1,1,1)
        _MoveSpeedU ("U Move Speed", Range(-6,6)) = 0.5
        _MoveSpeedV ("V Move Speed", Range(-6,6)) = 0.5
        _FogColor ("Lava Fog Color", Color) = (0.3, 0.4, 0.7, 1.0)
	}
	SubShader {
	    Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
    	ZWrite Off
    	LOD 200


		CGPROGRAM
    		#pragma surface surf Lambert finalcolor:mycolor vertex:myvert

    		sampler2D _MainTex;
    		fixed4 _Color;
    		fixed _MoveSpeedU;
    		fixed _MoveSpeedV;
    		fixed4 _FogColor;

    		struct Input {
    			float2 uv_MainTex;
    			half fog;
    		};

            void myvert (inout appdata_full v, out Input data) {

                  UNITY_INITIALIZE_OUTPUT(Input,data);
                  float4 hpos = UnityObjectToClipPos(v.vertex);
                  hpos.xy/=hpos.w;
                  data.fog = min (1, dot (hpos.xy, hpos.xy)*0.5);
             }


             void mycolor (Input IN, SurfaceOutput o, inout fixed4 color) {
                  fixed3 fogColor = _FogColor.rgb;
                  #ifdef UNITY_PASS_FORWARDADD
                  fogColor = 0;
                  #endif
                  color.rgb = lerp (color.rgb, fogColor, IN.fog);
            }

    		void surf (Input IN, inout SurfaceOutput o) {

    			fixed2 MoveScrolledUV = IN.uv_MainTex;

    			fixed MoveU = _MoveSpeedU * _Time;
    			fixed MoveV = _MoveSpeedV * _Time;

    			MoveScrolledUV += fixed2(MoveU, MoveV);

    			half4 c = tex2D (_MainTex, MoveScrolledUV);
    			o.Albedo = c.rgb * _Color;
    			o.Alpha = _Color.a;
    		}
    		ENDCG
    	}
    	FallBack "Diffuse"
}
