Shader "Custom/Stenciled"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 1
    }
    SubShader
    {
        Tags
        {
			"Queue" = "Geometry-1"
        }

        Pass
        {
            ColorMask 0
			ZWrite Off

			Stencil
			{
				Ref [_StencilID]
				Comp notEqual
				Pass Replace
			}
            
        }
    }
}
