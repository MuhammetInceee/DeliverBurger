using System.Threading.Tasks;
using MoreMountains.NiceVibrations;

public class GameManager : Singleton<GameManager>
{
    internal bool canHitHaptic = true;
    internal bool canSling = true;


    public void HitHaptic(HapticTypes type)
    {
        if(!canHitHaptic) return;
        print("Haptic");
        MMVibrationManager.Haptic(type);
    }

    public void StopGame()
    {
        canSling = false;
    }

    public async void ContinueGame()
    {
        await Task.Delay(20);
        canSling = true;
    }
}
