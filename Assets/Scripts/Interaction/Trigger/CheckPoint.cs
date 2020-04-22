using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class CheckPoint : MonoBehaviour
{
//    [SerializeField]    private Color onColor = Color.green;
//    [SerializeField]    private Color offColor = Color.red;
//    [SerializeField]    private GameObject flag;
//                        private Material flagMaterial;
    [SerializeField]    private GameObject bodyPlayer;
    [SerializeField]    private PlayerController playerControllerScript;

    [SerializeField]    private float radiusZone = 1f;
                        private bool isActivate = false;

    [SerializeField] private UnityEvent OnCheck = null;

    private void Awake()
    {
       //flagMaterial = flag.GetComponent<MeshRenderer>().material;
        //flagMaterial.SetColor("_BaseColor", offColor);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivate && (bodyPlayer.transform.position - transform.position).sqrMagnitude < radiusZone * radiusZone)
        {
            isActivate = true;
            //flagMaterial.SetColor("_BaseColor", onColor);
            playerControllerScript.UseCheckPointPosition(transform.position);
            OnCheck?.Invoke();
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
            //flagMaterial.SetColor("_BaseColor", onColor);
            isActivate = true;
        }
        else
        {
            //flagMaterial.SetColor("_BaseColor", offColor);
            isActivate = false;
        }
    }
}
