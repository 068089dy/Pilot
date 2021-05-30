Shader "Hidden/briSatConShader"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
		//Graphics.Blit���������src
		_MainTex("Source Texture", 2D) = "white" {}
		//����ֵ
		_Brightness("Brightness", Float) = 1
		//���Ͷ�
		_Saturation("Saturation", Float) = 1
		//�Աȶ�
		_Contrast("Contrast", Float) = 1
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			//CG�д������������Ӧ�ı���
			sampler2D _MainTex;
			half _Brightness;
			half _Saturation;
			half _Contrast;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            //sampler2D _MainTex;
			//������ɫ��������ת���Լ���ȡuvֵ  
			/*v2f vert(appdata_img v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);

				o.uv = v.texcoord;

				return o;
			}*/

            fixed4 frag (v2f i) : SV_Target
            {
				//�������
				fixed4 renderTex = tex2D(_MainTex, i.uv);

				// ����ֵ����
				fixed3 finalColor = renderTex.rgb * _Brightness;

				// �����ض�Ӧ������ֵ
				fixed luminance = 0.2125 * renderTex.r + 0.7154 * renderTex.g + 0.0721 * renderTex.b;
				//ʹ�ø�����ֵ����һ�����Ͷ�Ϊ0����ɫ
				fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
				//��֮ǰ����ɫ�����ɫ���в�ֵ���㣬�õ��������ͶȺ����ɫ
				finalColor = lerp(luminanceColor, finalColor, _Saturation);

				// ����һ���Աȶȶ�Ϊ0����ɫ
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				//��֮ǰ����ɫ�����ɫ���в�ֵ���㣬�õ������ԱȶȺ����ɫ
				finalColor = lerp(avgColor, finalColor, _Contrast);
				//fixed4 finalColor = fixed4(1.0, 1.0, 1.0, 1.0);
				//����������ɫ   
				return fixed4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}
