using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(LightBlendRenderer), PostProcessEvent.AfterStack, "Custom/LightBlend", false)]
public sealed class LightBlend: PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Blending of lights.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
    public TextureParameter lightTex = new TextureParameter();

    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value && blend.value > 0f && lightTex.value != null;
    }
}

public sealed class LightBlendRenderer : PostProcessEffectRenderer<LightBlend>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/LightBlend"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetTexture("_LightTex", settings.lightTex);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}