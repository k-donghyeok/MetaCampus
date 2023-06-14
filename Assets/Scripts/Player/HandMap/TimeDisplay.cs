using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    private void Start()
    {
        if (StageManager.Instance().IsExterior()
            || StageManager.Instance().GetName().StartsWith("Tutorial"))
        {
            text.gameObject.SetActive(false);
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        int time = Mathf.CeilToInt(StageManager.Instance().Time.RemainingTime);
        text.text = $"{time / 60:0}:{time % 60:00}";
    }
}
