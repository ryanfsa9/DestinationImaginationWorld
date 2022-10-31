Shader "Custom/WaterShader2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _BumpMap1 ("Bumpmap 1", 2D) = "bump" {}
        _BumpMap2 ("Bumpmap 2", 2D) = "bump" {}
        _BumpMap3 ("Bumpmap 3", 2D) = "bump" {}
        _N1Scale ("N1 Scale", Float) = 50
        _N1Height ("N1 Height", Float) = 10
        _Speed ("Wave Speed", Range(-10,10)) = 5
        _Radius ("Low Wave Radius", Float) = 10
        _HS ("Height Scale", Float) = 10
        _blend ("Wave Top Blending", Float) = 10
        [Toggle] _colorFromHeight("Color From Height", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        

        sampler2D _MainTex;
        sampler2D _BumpMap1;
        sampler2D _BumpMap2;
        sampler2D _BumpMap3;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap1;
            float2 uv_BumpMap2;
            float2 uv_BumpMap3;
            float3 localPos;
            float voronoiMap;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _N1Scale;
        float _N1Height;
        float _Speed;
        float _Radius;
        float _colorFromHeight;
        float _HS;
        float _blend;
        
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        inline float2 randomVector (float2 UV, float offset){
            float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
            UV = frac(sin(mul(UV, m)) * 46839.32);
            return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
        }
        float random (float2 uv){
                return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
        }
        float Voronoi(float2 UV, float AngleOffset, float CellDensity){
            float2 g = floor(UV * CellDensity);
            float2 f = frac(UV * CellDensity);
            float t = 8.0;
            float3 res = float3(8.0, 0.0, 0.0);
            float Out = 0;
            for(int y=-1; y<=1; y++)
            {
                for(int x=-1; x<=1; x++)
                {
                    float2 lattice = float2(x,y);
                    float2 offset = randomVector(lattice + g, AngleOffset);
                    float d = distance(lattice + offset, f);
                    if(d < res.x)
                    {
                        res = float3(d, offset.x, offset.y);
                        
                        Out = res.x;
                    }
                }
            }
            return Out;
        }
        float CalcHeight(float2 UV){
            float height = Voronoi(UV,_Speed*_Time.z,_N1Scale) * _N1Height;
            return height * _HS;
        }
        float3 CalcNormal(float2 UV){
            float base = CalcHeight(UV);
            float3 vectorX = float3(1 ,0 ,(CalcHeight(UV+float2(0.00001,0))-base)/0.00001);
            float3 vectorY = float3(0 ,1 ,(CalcHeight(UV+float2(0,0.00001))-base)/0.00001);
            return normalize(cross(vectorX,vectorY));
        }
        float3 NormalBlend(float3 A, float3 B){
            return normalize(float3(A.rg + B.rg, A.b * B.b));
        }
        float3 avgNormal(float2 UV){
            float3 mainNormal  = CalcNormal(UV);
            float3 upNormal    = CalcNormal(UV + float2(0,_blend));
            float3 downNormal  = CalcNormal(UV - float2(0,_blend));
            float3 leftNormal  = CalcNormal(UV - float2(_blend,0));
            float3 rightNormal = CalcNormal(UV - float2(_blend,0));
            return normalize((mainNormal+upNormal+downNormal+leftNormal+rightNormal)/4);
        }
        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            float height = CalcHeight(v.vertex.xz);
            float dist = sqrt(v.vertex.x*v.vertex.x+v.vertex.z*v.vertex.z);
            if(dist < _Radius){
                height *= dist * dist/_Radius/_Radius;
            }
            v.vertex.y += height;
            o.localPos = v.vertex.xyz;
            o.voronoiMap = height;
        }

        void surf (Input IN, inout SurfaceOutputStandard o){
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            if(_colorFromHeight == 0){
                o.Albedo = c.rgb;
            }
            else{
                o.Albedo = IN.voronoiMap;
            }
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            
            float3 mainNormal = avgNormal(IN.localPos.xz);
            float3 normal1 = UnpackNormal (tex2D (_BumpMap1, IN.uv_BumpMap1));
            float3 normal2 = UnpackNormal (tex2D (_BumpMap2, IN.uv_BumpMap2));
            float3 normal3 = UnpackNormal (tex2D (_BumpMap3, IN.uv_BumpMap3));
            o.Normal = NormalBlend(NormalBlend(mainNormal,normal1),NormalBlend(normal2,normal3));
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
