using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.CodeDom.Compiler;

[CustomEditor(typeof(GameManager))]
public class GameManagerGenerateButtons : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager manager = (GameManager)target;
        if (GUILayout.Button("Save LockIDs"))
        {
#if UNITY_EDITOR
            var interfaces = FindObjectsOfType<MonoBehaviour>().OfType<IHaveLockID>();
            LockManager.SaveIDs(interfaces.ToArray());
#endif
        }
    }
}

