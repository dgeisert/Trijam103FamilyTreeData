Shader "VertexColor/UnlitWindFog" {
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
            float4 worldPos = mul (unity_ObjectToWorld, v.vertex);
            float val = sin(_Time.z * 17 / 5 + worldPos.x / 5) / 60 + sin(_Time.z * 23 / 5 + worldPos.x / 5) / 60 + sin(_Time.z + worldPos.x / 5) / 60;
			//v.vertex.y += val;
			worldPos.x += val * min(v.vertex.y, 1);
			worldPos.z += val * min(v.vertex.y, 1);

            v.vertex = mul( unity_WorldToObject, worldPos );
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = IN.vertColor.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}