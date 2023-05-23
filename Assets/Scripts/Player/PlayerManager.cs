using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 서브시스템 간 상호작용을 위한 홀더
/// </summary>
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
