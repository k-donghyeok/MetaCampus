using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [Header("Skybox")]
    [SerializeField]
    private Skybox skyboxDay;
    [SerializeField]
    private Skybox skyboxNight;

    [Header("Directional Lights")]
    [SerializeField]
    private GameObject dirLightDay;
    [SerializeField]
    private GameObject dirLightNight;

    public void Initialize(bool day)
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