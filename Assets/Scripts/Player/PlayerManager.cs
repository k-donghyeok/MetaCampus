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

    [SerializeField]
    private InkPenManager pen = null;

    public PhoneManager Phone() => phone;

    public HandMapManager Map() => map;

    public InkPenManager Pen() => pen;

    /// <summary>
    /// <see cref="PlayerManager"/> 인스턴스
    /// </summary>
    public static PlayerManager Instance()
        => GameObject.FindGameObjectWithTag("Player").transform.root.GetComponent<PlayerManager>();

    /// <summary>
    /// 플레이어 실제 위치
    /// </summary>
    public static Transform InstanceOrigin()
        => Instance().xrOrigin;

    private void Start()
    {
        Phone().player = this;
        Map().player = this;
        Pen().player = this;
    }
}
