int radius;
static const float X_c = radius / 2;
static const float Y_c = radius / 2;

float4 drawColor;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR
{
    float dx = coords.x - 0.5f;
    float dy = coords.y - 0.5f;
    if(dx * dx + dy * dy <= 0.25f)
        return drawColor;
    else
        return float4(0.0f, 0.0f, 0.0f, 0.0f);
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}