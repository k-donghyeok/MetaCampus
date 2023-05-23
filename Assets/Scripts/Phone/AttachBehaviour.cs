using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 찍은 사진을 계획도에 붙이는 행동 관리
/// </summary>
public class AttachBehaviour
{
    private readonly PhoneManager owner;

    public AttachBehaviour(PhoneManager owner)
    {
        this.owner = owner;
    }

    private Texture2D photo = null;

    /// <summary>
    /// 저장된 사진 갱신
    /// </summary>
    public void UpdatePhoto(Texture2D photo)
    {
        this.photo = photo;
        owner.photoScreen.material.mainTexture = this.photo;
    }

    public void Update()
    {
        owner.Map.UpdatePhotoProjection(owner.transform);

        // allow stretch
    }
}
