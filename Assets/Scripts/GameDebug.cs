using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDebug 
{
    public static void AssertInTransform(bool isSuccess, Transform trans, string errorMessage)
    {
        if (!isSuccess)
        {
            string objectHierarchy = "";
            Transform o = trans;

            //while (o.transform.parent != null)
            uint i = 0;
            do
            {
                if (o != null)
                {
                    objectHierarchy += o.gameObject.name + " <- ";
                    o = o.parent;
                }
                ++i;
                if (i > 100)
                    break;
            } while (o != null);
            Debug.LogError(errorMessage + " in : " + objectHierarchy);
        }
    }
}
