Shader "Custom/Sprite Outline" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Color("Color", Color) = (1, 1, 1, 1)
	}
		// SS start

		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _MainTex;

	struct v2f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};

	v2f vert(appdata_base v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		return o;
	}


	fixed4 _Color;
	float4 _MainTex_TexelSize;

	fixed4 frag(v2f i) : COLOR
	{
		half4 c = tex2D(_MainTex, i.uv);
		c.rgb *= c.a;
		half4 outlineC = _Color;
		//outlineC.a *= ceil(c.a);
		outlineC.rgb *= outlineC.a;

		fixed alpha_up = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
		fixed alpha_down = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
		fixed alpha_right = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;
		fixed alpha_left = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;

		return lerp(lerp(c, outlineC, c.a != 1 && ((alpha_up == 1) || (alpha_down == 1) || (alpha_right == 1) || (alpha_left == 1))), c, 1-outlineC);
	}

		ENDCG
	}
	}
		// SS end
		FallBack "Diffuse"
}