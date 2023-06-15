public class FenceGate_Tutorial : FenceGateCheck
{
    protected override bool CheckGate()
    {
        return MySceneManager.GetCleared(MySceneManager.SCENENAME.Engineering);
    }
}
