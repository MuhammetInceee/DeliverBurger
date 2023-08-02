using Cinemachine;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera1;
    [SerializeField] private CinemachineVirtualCamera camera2;
    
    public void HapticOn()
    {
        GameManager.Instance.canHitHaptic = true;
    }

    public void HapticOff()
    {
        GameManager.Instance.canHitHaptic = false;
    }

    public void CameraOneButton()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }
    
    public void CameraTwoButton()
    {
        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
    }
}
