using System.Linq;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 유니티 에디터에서 GameManager의 Inspector에 버튼을 추가
/// </summary>
[CustomEditor(typeof(GameManager))]
public class GameManagerGenerateButtons : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Inspector로 보고 있는 GameManager 인스턴스
        GameManager manager = (GameManager)target;

#if UNITY_EDITOR
        // 이를 사용하면 Save LockIDs란 버튼이 추가됨
        // 그리고 버튼을 누르면 안의 내용이 한번 실행됨
        if (GUILayout.Button("Save LockIDs"))
        {
            // 이 장면의 모든 MonoBehaviour 중 IHaveLockID 인터페이스를 구현하는 것만 가져옴
            var interfaces = FindObjectsOfType<MonoBehaviour>().OfType<IHaveLockID>();
            // 그리고 SaveIDs를 실행해 게임 파일로 저장
            LockManager.SaveIDs(interfaces.ToArray());
        }
#endif
    }
}

