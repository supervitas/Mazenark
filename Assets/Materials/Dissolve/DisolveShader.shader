Shader "Custom/Dissolving" {
    Properties {
        _MainTex ("Texture (RGB)", 2D) = "white" {}
        _SliceGuide ("Slice Guide (RGB)", 2D) = "white" {}
        _SliceAmount ("Slice Amount", Range(0.0, 1.0)) = 0.0     
        _BurnSize ("Burn Size", Range(0.0, 1.0)) = 0.4
        _BurnRamp ("Burn Ramp (RGB)", 2D) = "white" {}
    }
        
        SubShader {
          Tags { "RenderType" = "Opaque" }
          Cull Off
          CGPROGRAM
          //if you're not planning on using shadows, remove "addshadow" for better performance
          #pragma surface surf Lambert addshadow vertex:vert
          #pragma target 3.0
          
          struct Input {
              float2 uv_MainTex;
              float2 uv_SliceGuide;
              float _SliceAmount;
          };    
    
          sampler2D _MainTex;
          sampler2D _SliceGuide;
          float _SliceAmount;
          sampler2D _BurnRamp;
          float _BurnSize;
        
        void vert (inout appdata_full v) {       
              v.vertex.x += v.vertex.x * _SliceAmount;
              v.vertex.z += v.vertex.z * _SliceAmount;
              v.vertex.y += v.vertex.y * _SliceAmount;               
        }
    
        void surf (Input IN, inout SurfaceOutput o) {
             clip(tex2D (_SliceGuide, IN.uv_SliceGuide).rgb - _SliceAmount);
             o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
     
             half test = tex2D (_SliceGuide, IN.uv_MainTex).rgb - _SliceAmount;
             if(test < _BurnSize && _SliceAmount > 0 && _SliceAmount < 1) {
                o.Emission = tex2D(_BurnRamp, float2(test *(1/_BurnSize), 0));
             o.Albedo *= o.Emission;
             }
          }
          ENDCG
        } 
        Fallback "Diffuse"
      }