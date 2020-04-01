using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class CheckPoint : MonoBehaviour
{
    [SerializeField]    private Color onColor = Color.green;
    [SerializeField]    private Color offColor = Color.red;
    [SerializeField]    private GameObject flag;
                        private Material flagMaterial;
    [SerializeField]    private GameObject bodyPlayer;
    [SerializeField]    private PlayerController playerControllerScript;

                        float radiusZone = 1f;
                        private bool isActivate = false;

    private void Awake()
    {
        flagMaterial = flag.GetComponent<MeshRenderer>().material;
        flagMaterial.SetColor("_BaseColor", offColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivate && (bodyPlayer.transform.position - transform.position).sqrMagnitude < radiusZone)
        {
            isActivate = true;
            flagMaterial.SetColor("_BaseColor", onColor);
            playerControllerScript.UseCheckPointPosition(transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (!isActivate)
            Gizmos.DrawWireSphere(transform.position, radiusZone);
    }


    public void SetFlag(bool flag)
    {
        if (flag)
        {
            flagMaterial.SetColor("_BaseColor", onColor);
            isActivate = true;
        }
        else
        {
            flagMaterial.SetColor("_BaseColor", offColor);
            isActivate = false;
        }
    }
}
