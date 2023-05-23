using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform xrOrigin = null;

    [Header("SubSystems")]
    [SerializeField]
    private PhoneManager phone = null;

    [SerializeField]
    private HandMapManager map = null;

    public PhoneManager Phone() => phone;

    public HandMapManager Map() => map;

    private void Start()
    {
        Phone().player = this;
        Map().player = this;
    }
}
