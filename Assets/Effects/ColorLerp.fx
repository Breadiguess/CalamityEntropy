sampler uImage : register(s0);

float4 EnchantedFunction(float4 baseColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 colory = tex2D(uImage, frac(coords));
    return lerp(baseColor, float4(1, 1, 1, 1), (colory.r + colory.g + colory.b) / 3) * float4(colory.r, colory.g, colory.b, colory.r);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 EnchantedFunction();
    }
}