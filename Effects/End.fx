sampler2D RenderTargetSampler : register(s0); // RenderTarget ����
sampler2D ImageSampler : register(s1); // Ŀ��ͼƬ����

struct PS_INPUT
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
};

float4 MainPS(PS_INPUT input) : SV_Target
{
    // ��RenderTarget��ȡ��ǰ���ص���ɫ
    float4 renderTargetColor = tex2D(RenderTargetSampler, input.TexCoord);
    float4 imageColor = tex2D(ImageSampler, input.TexCoord);
    // �����ǰ��������ȫ��͸����
    if (renderTargetColor.a > 0.0f)
    {
        // ��Ŀ��ͼƬ��ȡ��Ӧ�������ɫ
        
        // ��RenderTarget����ɫ�滻ΪĿ��ͼƬ����ɫ
        return imageColor * renderTargetColor.a;
    }
    else
    {
        // ����ԭRenderTarget����ɫ
        return renderTargetColor;
    }
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}
