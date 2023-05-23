using System.Linq;
using UnityEngine;

public class PlanTextureManager
{
    /// <summary>
    /// 계획 텍스쳐
    /// </summary>
    public Texture2D PlanTexture { get; private set; }

    private readonly HandMapManager owner = null;

    public PlanTextureManager(HandMapManager owner)
    {
        this.owner = owner;
        PlanTexture = new Texture2D(2048, 2048, TextureFormat.ARGB32, false);
        PlanTexture.SetPixels32(Enumerable.Repeat(new Color32(0, 0, 0, 0), PlanTexture.width * PlanTexture.height).ToArray());
        PlanTexture.Apply();

        owner.UpdateTexture(PlanTexture);
    }

    
    public struct PhotoTransform
    {
        public Vector2 offset;
        public float rotation;
        public float scale;
    }

    public void OverlayPhoto(Texture2D photo, PhotoTransform transform)
    {
        transform.offset += new Vector2(PlanTexture.width, PlanTexture.height) * 0.5f;

        int photoWidth = photo.width;
        int photoHeight = photo.height;

        Color32[] planPixels = PlanTexture.GetPixels32();
        Color32[] photoPixels = photo.GetPixels32();

        int planWidth = PlanTexture.width;
        int planHeight = PlanTexture.height;

        // Convert rotation from degrees to radians
        float rotationRad = transform.rotation * Mathf.Deg2Rad;

        // Calculate cos and sin values for the rotation angle
        float cosAngle = Mathf.Cos(rotationRad);
        float sinAngle = Mathf.Sin(rotationRad);

        for (int y = 0; y < photoHeight; y++)
        {
            for (int x = 0; x < photoWidth; x++)
            {
                // Calculate transformed coordinates based on offset, rotation, and scale
                float transformedX = (x - (photoWidth / 2)) * cosAngle - (y - (photoHeight / 2)) * sinAngle;
                float transformedY = (x - (photoWidth / 2)) * sinAngle + (y - (photoHeight / 2)) * cosAngle;
                transformedX = transformedX * transform.scale + transform.offset.x;
                transformedY = transformedY * transform.scale + transform.offset.y;

                // Convert transformed coordinates to integer values
                int planX = Mathf.RoundToInt(transformedX);
                int planY = Mathf.RoundToInt(transformedY);

                // Ensure the transformed coordinates are within the bounds of the PlanTexture
                if (planX >= 0 && planX < planWidth && planY >= 0 && planY < planHeight)
                {
                    // Calculate the index of the pixel in the PlanTexture and photoTexture
                    int planIndex = planY * planWidth + planX;
                    int photoIndex = y * photoWidth + x;

                    // Set the pixel color in the PlanTexture based on the corresponding pixel color in the photoTexture
                    planPixels[planIndex] = photoPixels[photoIndex];
                }
            }
        }

        // Apply the modified pixels back to the PlanTexture
        PlanTexture.SetPixels32(planPixels);
        PlanTexture.Apply();

        owner.UpdateTexture(PlanTexture);
    }
}