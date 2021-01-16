// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "VertexColor/UnlitFog" {
    Properties {
        _FogColor ("Fog Color", Color) = (0,0,0,1)
		_FogDist ("Fog Distance", float) = 100
    }
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0

		float4 _FogColor;
		float _FogDist;

		struct Input {
			float4 vertColor;
		};

		void vert(inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float dist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, v.vertex));
			o.vertColor = lerp(v.color, _FogColor, dist / _FogDist);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = IN.vertColor.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}