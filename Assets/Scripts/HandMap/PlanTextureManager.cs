using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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



}