public class FenceGate_Boss : FenceGateCheck
{
    protected override bool CheckGate()
    {
        return MySceneManager.GetCleared(MySceneManager.SCENENAME.Medical)
            && MySceneManager.GetCleared(MySceneManager.SCENENAME.Arts);
    }
}
