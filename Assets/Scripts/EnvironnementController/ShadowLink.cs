using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowLink : MonoBehaviour
{
    [SerializeField] private GameObject shadow = null;
    [SerializeField] private bool shadowMovementsToObject = true;
    [SerializeField] private bool objectMovementsToShadow = false;

    private Vector3 lastObjectLocation;
    private Vector3 lastShadowLocation;

    private void Awake()
    {
        GameDebug.AssertInTransform(shadow != null, transform, "Shadow shall not be null");

        lastObjectLocation = transform.position;
        lastShadowLocation = shadow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectMovementsToShadow && lastObjectLocation != transform.position)
        {
            shadow.transform.position += transform.position - lastObjectLocation;

            lastShadowLocation = shadow.transform.position;
            lastObjectLocation = transform.position;
        }

        if (shadowMovementsToObject && lastShadowLocation != shadow.transform.position)
        {
            transform.position += shadow.transform.position - lastShadowLocation;

            lastShadowLocation = shadow.transform.position;
            lastObjectLocation = transform.position;
        }
    }
}
