using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private SubMenu[] subMenus = null;

    private float oldTimeScale;
    private AnimatorUpdateMode oldAnimatorUpdateMode;

    [SerializeField]
    private PlayerController controller = null;
    [SerializeField]
    private InputManager inputManager = null;

    private bool isMenuOpened = false;

    private bool isSubmenuOpened()
    {
        foreach (SubMenu sub in subMenus)
        {
            if (sub.isActivate)
            {
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        subMenus = GetComponentsInChildren<SubMenu>(true);
    }

    private void Start()
    {
        GameDebug.AssertInTransform(subMenus != null, transform, "subMenus should not be null");
        GameDebug.AssertInTransform(subMenus.Length != 0, transform, "subMenus should have at least one element");
        GameDebug.AssertInTransform(subMenus[0] != null, transform, "First element of subMenus should not be null");

        GameDebug.AssertInTransform(controller != null, transform, "controller should not be null");
        GameDebug.AssertInTransform(inputManager != null, transform, "inputManager shoud not be null");
    }

    public void OpenMenu(SubMenu sub)
    {
        // === Saving old stats to restore them later === //

        // Saving and updating animator update mode
        oldAnimatorUpdateMode = controller.shadowAnimator.updateMode;
        controller.SetShadowAnimatorNormalMode(AnimatorUpdateMode.Normal);

        // Saving and updating time scale
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // Disabling inputs 
        controller.enabled = false;
        inputManager.enabled = false;

        subMenus[0].EnableMenu();
    }

    public void CloseMenu(SubMenu sub)
    {
        // Restoring animation
        controller.SetShadowAnimatorNormalMode(oldAnimatorUpdateMode);
        // Restoring time scale
        Time.timeScale = oldTimeScale;

        // Enabling inputs
        controller.enabled = true;
        inputManager.enabled = true;

        sub.DisableMenu();
    }

    public void CloseAllMenus()
    {
        // Close all opened menus
        foreach (SubMenu sub in subMenus)
        {
            CloseMenu(sub);
        }
        isMenuOpened = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            if (isSubmenuOpened())
            {
                CloseAllMenus();
            }
            else
            {
                // Open the first menu
                if (subMenus.Length != 0)
                {
                    OpenMenu(subMenus[0]);
                    isMenuOpened = true;
                }
            }
        }

        if (isMenuOpened && !isSubmenuOpened())
        {
            CloseAllMenus();
        }
    }
}
