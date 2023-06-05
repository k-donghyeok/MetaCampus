using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class LightManager : MonoBehaviour
{
    [Header("Volume Profiles")]
    [SerializeField]
    private VolumeProfile profileDay;
    [SerializeField]
    private VolumeProfile profileNight;

    [Header("Skyboxes")]
    [SerializeField]
    private Material skyboxDay;
    [SerializeField]
    private Material skyboxNight;

    [Header("Directional Lights")]
    [SerializeField]
    private GameObject dirLightDay;
    [SerializeField]
    private GameObject dirLightNight;

    public void Initialize(bool day)
    {
        // 후처리 필터 Volume Profile 설정
        transform.parent.GetComponent<Volume>().profile
            = day ? profileDay : profileNight;

        // Skybox 설정
        RenderSettings.skybox
            = day ? skyboxDay : skyboxNight;

        ChangeDirectionalLight(day);

        void ChangeDirectionalLight(bool day)
        {
            if (day)
            {
                dirLightDay.SetActive(true);
                dirLightNight.SetActive(false);
            }
            else
            {
                dirLightDay.SetActive(false);
                dirLightNight.SetActive(true);
            }
        }
    }
}