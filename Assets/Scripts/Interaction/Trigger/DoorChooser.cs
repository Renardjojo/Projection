using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorChooser : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial" && PlayerPrefs.GetInt("Tutorial") == 1)
            GetComponent<Trigger>().IsOn = true;
    }
}
