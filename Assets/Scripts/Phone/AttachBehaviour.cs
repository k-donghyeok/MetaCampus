using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ ��ȹ���� ���̴� �ൿ ����
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
    /// ����� ���� ����
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
