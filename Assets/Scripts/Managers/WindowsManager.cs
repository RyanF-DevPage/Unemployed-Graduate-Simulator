using UnityEngine;
using UnityEngine.UI;

public class WindowsManager : MonoBehaviour
{
    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
    }
    
    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }


}
