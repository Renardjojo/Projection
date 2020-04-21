using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ChangeSubMenuButton : MonoBehaviour
{
    [SerializeField]
    private SubMenu currentSubMenu = null;

    [SerializeField]
    private SubMenu newSubMenu = null;

    private void Awake()
    {
        UnityEngine.UI.Button btn = GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (currentSubMenu != null)
            currentSubMenu.DisableMenu();

        if (newSubMenu != null)
            newSubMenu.EnableMenu();
    }
}
