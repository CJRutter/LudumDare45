using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LightCamera : BaseBehaviour
{
    public override void Init()
    {
        base.Init();

        if(Target == null)
            Target = new RenderTexture(Screen.width, Screen.height, 0);
        //Target.width = Screen.width;
        //Target.height = Screen.height;

        Camera.targetTexture = Target;

        LightPostProcVolume.profile.TryGetSettings<LightBlend>(out lightBlendSettings);
        lightBlendSettings.lightTex.value = Target;
    }

    void Update()
    {
        if (Target.width != Screen.width || Target.height != Screen.height)
        {
            Camera.targetTexture.Release();
            Target = new RenderTexture(Screen.width, Screen.height, 0);
            Camera.targetTexture = Target;
            lightBlendSettings.lightTex.value = Target;
        }
    }

    #region Properties
    #endregion Properties        

    #region Fields
    public Camera Camera;
    public RenderTexture Target;
    public PostProcessVolume LightPostProcVolume;

    private LightBlend lightBlendSettings;
    #endregion Fields
}
