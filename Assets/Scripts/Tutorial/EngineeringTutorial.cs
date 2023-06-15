using UnityEngine;

public class EngineeringTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject firstDay;

    [SerializeField]
    private GameObject afterFail;

    public const string FAIL_CHECK = "EngineeringFailed";

    private void Start()
    {
        bool failed = GameManager.Instance().Save.LoadValue(FAIL_CHECK, false);
        if (failed || !GameManager.Instance().IsDaytime()) firstDay.SetActive(false);
        if (!failed) afterFail.SetActive(false);

        if (StageManager.Instance().Initialized)
            AppendFailCheckEvent();
        else
            StageManager.Instance().OnStageLoad += (stage) => { AppendFailCheckEvent(); };

        void AppendFailCheckEvent()
        {
            StageManager.Instance().Time.OnTimeOver
            += () => { GameManager.Instance().Save.SaveValue(FAIL_CHECK, true); };
        }
    }

}
