using UnityEngine;
using UnityEngine.XR;

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

    private bool lastTrigger = false;

    public void Update(InputDevice device)
    {
        owner.Map.UpdatePhotoProjection(owner.attachGO.transform);

        if (device.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
        {
            if (triggerValue > 0.9f)
            {
                if (!lastTrigger)
                {
                    bool attach = AttemptAttach(0.2f);
                    if (attach)
                    {
                        owner.AttachAction();
                        return;
                    }
                }
                lastTrigger = true;
            }
            else lastTrigger = false;
        }
        else lastTrigger = false;

    }

    public bool AttemptAttach(float leniency)
        => owner.Map.RequestPhotoAttach(photo, owner.attachGO.transform, leniency);

}
