using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ResetSaveButton : MonoBehaviour
{
    public void DeleteAllSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
