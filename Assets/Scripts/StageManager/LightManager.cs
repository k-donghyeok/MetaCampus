using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightManager : MonoBehaviour
{
    [Header("Volume Profiles")]
    [SerializeField]
    private VolumeProfile profileDay = null;
    [SerializeField]
    private VolumeProfile profileNight = null;

    [Header("Skyboxes")]
    [SerializeField]
    private Material skyboxDay = null;
    [SerializeField]
    private Material skyboxNight = null;

    [Header("Directional Lights")]
    [SerializeField]
    private GameObject dirLightDay = null;
    [SerializeField]
    private GameObject dirLightNight = null;

    [Header("Lightmaps")]
    [SerializeField]
    private Texture2D[] lightmapsDayDir = new Texture2D[0];
    [SerializeField]
    private Texture2D[] lightmapsDayColor = new Texture2D[0];
    [SerializeField]
    private Texture2D[] lightmapsNightDir = new Texture2D[0];
    [SerializeField]
    private Texture2D[] lightmapsNightColor = new Texture2D[0];

    public void Initialize(bool day)
    {
        // 후처리 필터 Volume Profile 설정
        transform.parent.GetComponent<Volume>().profile
            = day ? profileDay : profileNight;

        // Skybox 설정
        RenderSettings.skybox
            = day ? skyboxDay : skyboxNight;

        // Directional Light 설정
        dirLightDay.SetActive(day);
        dirLightNight.SetActive(!day);

        // Ambient Light 설정
        RenderSettings.ambientSkyColor = day ? new Color(0.6f, 0.7f, 1.0f) : new Color(0.3f, 0.3f, 0.6f);

        // Lightmap 설정
        LightMapSetup(day);

        // ReflectionProbe 다시 베이크
        var probes = GetComponents<ReflectionProbe>();
        foreach (var probe in probes) probe.RenderProbe();

        void LightMapSetup(bool day)
        {
            List<LightmapData> lData = new();
            if (day)
                for (int i = 0; i < lightmapsDayDir.Length; ++i)
                    lData.Add(new() { lightmapDir = lightmapsDayDir[i], lightmapColor = lightmapsDayColor[i] });
            else
                for (int i = 0; i < lightmapsNightDir.Length; ++i)
                    lData.Add(new() { lightmapDir = lightmapsNightDir[i], lightmapColor = lightmapsNightColor[i] });
            if (lData.Count < 1) return;

            LightmapSettings.lightmaps = lData.ToArray();
        }
    }
}