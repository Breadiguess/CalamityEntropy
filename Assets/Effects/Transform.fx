sampler uImage : register(s0); // SpriteBatch.Draw 的内容会自动绑定到 s0
sampler uTransformImage : register(s1); // 用于获取颜色的调色板
float uTime; // 实现调色板的滚动效果
float4 color;
float4 baseColor = float4(1, 1, 1, 1);
float strength = 0.6;
// 给附魔效果专门配置的
float4 EnchantedFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 colory = tex2D(uImage, coords) * baseColor;

	// any 为 false 即透明色，不能改
    if (!any(colory))
    {
        return colory;
    }
	float alpha = colory.a;
    colory.a = 1;
	// 根据 uTextImage 坐标以及 uTime 的值获取在调色板上的坐标，注意要 %1.0 以确保他在 [0, 1) 区间内
    float2 barCoord = float2((coords.x * 0.2 - uTime * 0.5) % 1.0, (coords.y * 0.2 + uTime) % 1.0);
    if (barCoord.x < 0)
    {
        barCoord.x = 1 + barCoord.x;
    }
	
	// 在调色板上选择颜色
    return clamp((tex2D(uTransformImage, barCoord) * strength * color + colory) * alpha, 0, 1);
}

technique Technique1
{
    pass EnchantedPass
    {
        PixelShader = compile ps_2_0 EnchantedFunction();
    }
}