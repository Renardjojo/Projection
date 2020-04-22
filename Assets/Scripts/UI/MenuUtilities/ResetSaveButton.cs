using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ResetSaveButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(DeleteAllSaves);
    }

    public void DeleteAllSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
