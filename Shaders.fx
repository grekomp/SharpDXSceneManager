

struct VS_IN
{
	float3 pos : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;
	float3 worldPos : TEXCOORD2;
};

cbuffer Transforms
{
	float4x4 worldViewProj;
	float4x4 worldView;
	float4x4 world;
}

cbuffer Material {
	float4 lightPos;
	float4 diffuseColor;
	float4 emissionColor;
}

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;

	output.pos = mul(float4(input.pos, 1), worldViewProj);
	output.normal = mul(input.normal, (float3x3)world);
	output.worldPos = mul(float4(input.pos, 1), world).xyz;
	output.uv = input.uv;

	return output;
}

float4 PS(PS_IN input) : SV_Target
{
	float3 L = normalize(lightPos.xyz - input.worldPos);
	float3 N = normalize(input.normal);

	float3 diffuse = diffuseColor.xyz * saturate(dot(N,L));

	return float4(diffuse + emissionColor, 1);
}
