sampler TextureSampler : register(s0);

float2 Center;
float Strength;
float AspectRatio;
float FadeOutDistance; // ������ʼ�İ뾶����(0-1)
float FadeOutWidth;    // �������
float2 TexOffset;
float enhanceLightAlpha;

float4 PixelShaderFunction(float4 baseColor : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // ���������Կ��ǿ�߱�
    float2 adjustedTexCoord = texCoord - Center;
    adjustedTexCoord.y /= AspectRatio;
    
    // ���㵱ǰ�㵽���ĵľ���
    float distance = length(adjustedTexCoord);
    
    // ����͸���� (0=��ȫ͸��, 1=��͸��)
    float alpha = 1.0;
    if (distance > FadeOutDistance)
    {
        alpha = 1.0 - smoothstep(FadeOutDistance, FadeOutDistance + FadeOutWidth, distance);
    }
    
    // ��������Ť��
    float angle = atan2(adjustedTexCoord.y, adjustedTexCoord.x);
    angle += Strength * distance;
    
    adjustedTexCoord.x = cos(angle) * distance;
    adjustedTexCoord.y = sin(angle) * distance;
    
    // �ָ���߱ȵ���
    adjustedTexCoord.y *= AspectRatio;
    adjustedTexCoord += Center;
    
    // ��������Ӧ��͸����
    float4 color = tex2D(TextureSampler, adjustedTexCoord + TexOffset);
    if(color.r > enhanceLightAlpha)
    {
        float z = enhanceLightAlpha + (color.r - enhanceLightAlpha) * 3.5;
        color = float4(z, z, z, color.a);
    }
    color *= float4(alpha, alpha, alpha, alpha);
    
    return color * baseColor;
}

technique VortexTechnique
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}