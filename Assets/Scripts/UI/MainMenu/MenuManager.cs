using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private SubMenu[] subMenus = null;

    private bool isSubmenuOpened()
    {
        foreach (SubMenu sub in subMenus)
        {
            if (sub.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        GameDebug.AssertInTransform(subMenus != null, transform, "subMenus should not be null");
        GameDebug.AssertInTransform(subMenus.Length != 0, transform, "subMenus should have at least one element");
        GameDebug.AssertInTransform(subMenus[0] != null, transform, "First element of subMenus should not be null");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (isSubmenuOpened())
            {
                foreach (SubMenu sub in subMenus)
                {
                    sub.DisableMenu();
                }
            }
            else
            {
                if (subMenus.Length != 0)
                {
                    subMenus[0].EnableMenu();
                }
            }
        }
    }
}
