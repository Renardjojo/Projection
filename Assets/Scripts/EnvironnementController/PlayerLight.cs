using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField, Range(0,10)]
    private float radius = 1f;
    [SerializeField]
    private float sensibility = 0.1f;

    private Vector3 defaultPosition;
    private Vector2 relativePosition;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetLightX(float value)
    {
        relativePosition.x = value * radius;
        UpdateLightLocation();
    }
    public void SetLightY(float value)
    {
        relativePosition.y = value * radius;
        UpdateLightLocation();
    }

    public void MoveLightX(float value)
    {
        relativePosition.x += value * sensibility;
        UpdateLightLocation();
    }
    public void MoveLightY(float value)
    {
        relativePosition.y += value * sensibility;
        UpdateLightLocation();
    }

    public void UpdateLightLocation()
    {
        relativePosition = Vector2.ClampMagnitude(relativePosition, radius);

        transform.localPosition = new Vector3(defaultPosition.x + relativePosition.x, defaultPosition.y + relativePosition.y, defaultPosition.z);
    }
}
