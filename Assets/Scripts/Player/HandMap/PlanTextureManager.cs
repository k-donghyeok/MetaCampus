using System.IO;
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
        LoadPlan(StageManager.Instance().GetID());

        owner.UpdateTexture(PlanTexture);
        GameManager.Instance().Save.OnSaveToPref += SavePlanWithPrefSave;
        StageManager.Instance().OnStageUnload += (stage) =>
        {
            SavePlan(stage.GetID());
            GameManager.Instance().Save.OnSaveToPref -= SavePlanWithPrefSave;
        };
    }

    public void ResetPlan()
    {
        if (StageManager.Instance().IsExterior())
        {
            // 지정된 캠퍼스맵 텍스쳐를 사용
            var canvasMap = Resources.Load("Textures/Maps/Campus-map-text") as Texture2D;
            var colors = canvasMap.GetPixels32();
            PlanTexture.SetPixels32(colors);
        }
        else
        {
            // 하얗고 투명한 색으로 채움
            PlanTexture.SetPixels32(Enumerable.Repeat(new Color32(255, 255, 255, 0),
                PlanTexture.width * PlanTexture.height).ToArray());
        }
        PlanTexture.Apply();
    }

    private void LoadPlan(string name)
    {
        bool used = GameManager.Instance().Save.LoadValue($"{name}MapUsed", false);
        if (!used) goto Reset;
        string path = GetPath(name);
        if (!File.Exists(path)) goto Reset;

        var bytes = File.ReadAllBytes(path);
        PlanTexture.LoadImage(bytes);
        PlanTexture.Apply();
        return;

        Reset:
        ResetPlan();
        GameManager.Instance().Save.SaveValue($"{name}MapUsed", true);
    }

    private void SavePlanWithPrefSave(SaveManager save)
        => SavePlan(StageManager.Instance().GetID());

    private void SavePlan(string name)
    {
        string path = GetPath(name);
        var bytes = PlanTexture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }

    private string GetPath(string name)
        => $"{Application.persistentDataPath}/Plan-{name}.png";

    public struct PhotoTransform
    {
        public Vector2 offset;
        public float rotation;
        public float scale;
    }

    /// <summary>
    /// offset, rotation, scale 모두 사용해 텍스쳐를 그림.
    /// <para>사진을 붙이는 데 사용.</para>
    /// </summary>
    public void PastePhoto(Texture2D photo, PhotoTransform transform)
    {
        int planWidth = PlanTexture.width;
        int planHeight = PlanTexture.height;

        Color32[] planPixels = PlanTexture.GetPixels32();
        Color32[] photoPixels = photo.GetPixels32();

        transform.offset += new Vector2(planWidth, planHeight) * 0.5f;

        // Convert rotation from degrees to radians
        float rotationRad = transform.rotation * Mathf.Deg2Rad;

        // Calculate cos and sin values for the rotation angle
        float cosAngle = Mathf.Cos(rotationRad);
        float sinAngle = Mathf.Sin(rotationRad);

        float invScale = 1f / transform.scale;

        // Get the entire pixel array of the photo texture
        int photoWidth = photo.width;
        int photoHeight = photo.height;

        for (int planY = 0; planY < planHeight; planY++)
        {
            for (int planX = 0; planX < planWidth; planX++)
            {
                // Calculate the inverse transformation to get the corresponding pixel in the photoTexture
                float transformedX = ((planX - transform.offset.x) * invScale) * cosAngle + ((planY - transform.offset.y) * invScale) * sinAngle + (photoWidth * 0.5f);
                float transformedY = -((planX - transform.offset.x) * invScale) * sinAngle + ((planY - transform.offset.y) * invScale) * cosAngle + (photoHeight * 0.5f);

                // Calculate the normalized texture coordinates in the range of [0, 1]
                float normalizedX = transformedX / photoWidth;
                float normalizedY = 1f - (transformedY / photoHeight); // Invert Y-axis

                // Convert normalized coordinates to pixel coordinates in the range of [0, photoWidth-1] and [0, photoHeight-1]
                int photoX = Mathf.RoundToInt((1f - normalizedX) * (photoWidth - 1)); // Fix horizontal flipping
                int photoY = Mathf.RoundToInt(normalizedY * (photoHeight - 1));

                // Check if the transformed coordinates are within the bounds of the photoTexture
                if (photoX >= 0 && photoX < photoWidth && photoY >= 0 && photoY < photoHeight)
                {
                    // Calculate the index of the pixel in the PlanTexture and photoTexture
                    int planIndex = planY * planWidth + planX;
                    int photoIndex = photoY * photoWidth + photoX;

                    // Set the pixel color in the planPixels array
                    planPixels[planIndex] = photoPixels[photoIndex];
                }
            }
        }

        // Set the entire planPixels array to the plan texture
        PlanTexture.SetPixels32(planPixels);
        PlanTexture.Apply();

        owner.UpdateTexture(PlanTexture);
    }



    /// <summary>
    /// offset만 사용해 텍스쳐를 그림.
    /// <para>펜툴에 사용.</para>
    /// </summary>
    public void DrawPen(Vector2 lastOffset, Vector2 offset)
    {
        int planWidth = PlanTexture.width;
        int planHeight = PlanTexture.height;

        Vector2 originOffset = new(planWidth * 0.5f, planHeight * 0.5f);
        offset += originOffset;
        lastOffset += originOffset;

        const int PEN_SIZE = 12;
        for (float f = 0.00f; f < 1.00f; f += 0.03f)
            DrawRect(Vector2.Lerp(offset, lastOffset, f));

        PlanTexture.Apply();

        void DrawRect(Vector2 o)
        {
            for (int y = 0; y < PEN_SIZE; ++y)
            {
                for (int x = 0; x < PEN_SIZE; ++x)
                {
                    int planX = Mathf.RoundToInt(x + o.x - (PEN_SIZE >> 1));
                    int planY = Mathf.RoundToInt(y + o.y - (PEN_SIZE >> 1));
                    if (planX >= 0 && planX < planWidth && planY >= 0 && planY < planHeight)
                    {
                        var planPixel = PlanTexture.GetPixel(planX, planY);
                        planPixel.g = 0f;
                        planPixel.b = 0f;
                        planPixel.a = 1f;
                        PlanTexture.SetPixel(planX, planY, planPixel);
                    }
                }
            }
        }
    }
}