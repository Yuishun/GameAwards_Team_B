using UnityEngine;

/// <summary>
/// attach this script to effect camera screen with rain drops
/// using Rain.shader
/// 
/// original RainDrops Shader
/// https://github.com/ya7gisa0/Unity-Raindrops/blob/master/Raindrop/Assets/Raindrop.shader
/// </summary>
public class ScreenSpaceEffect : MonoBehaviour
{
    [SerializeField]
    private Shader shader; // apply RainDrops.shader
    [SerializeField, Range(0, 1)]
    private float rainAmount = 0.5f;
    [SerializeField, Range(0, 1)]
    private float lightning = 1.0f;
    [SerializeField, Range(0, 1)]
    private float vignette = 1.0f;
    private Material mat;

    private void Awake()
    {
        mat = new Material(shader);
    }


    private void Update()
    {
        mat.SetFloat("_RainAmount", rainAmount);
        mat.SetFloat("_Lightning", lightning);
        mat.SetFloat("_Vignette", vignette);

    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat);
    }

}
