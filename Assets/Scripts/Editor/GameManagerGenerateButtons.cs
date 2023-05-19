using System.Linq;
using UnityEngine;
using UnityEditor;

/// <summary>
/// ����Ƽ �����Ϳ��� GameManager�� Inspector�� ��ư�� �߰�
/// </summary>
[CustomEditor(typeof(GameManager))]
public class GameManagerGenerateButtons : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Inspector�� ���� �ִ� GameManager �ν��Ͻ�
        GameManager manager = (GameManager)target;

#if UNITY_EDITOR
        // �̸� ����ϸ� Save LockIDs�� ��ư�� �߰���
        // �׸��� ��ư�� ������ ���� ������ �ѹ� �����
        if (GUILayout.Button("Save LockIDs"))
        {
            // �� ����� ��� MonoBehaviour �� IHaveLockID �������̽��� �����ϴ� �͸� ������
            var interfaces = FindObjectsOfType<MonoBehaviour>().OfType<IHaveLockID>();
            // �׸��� SaveIDs�� ������ ���� ���Ϸ� ����
            LockManager.SaveIDs(interfaces.ToArray());
        }
#endif
    }
}

