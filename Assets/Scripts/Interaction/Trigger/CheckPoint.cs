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
                                float radiusZone = 1f;
                        private bool isActivate = false;
    [SerializeField]    private UnityEvent OnCheckPointIsActivate;
    [SerializeField]    private UnityEvent OnPlayerRespawn;
    [SerializeField]    private UnityEvent OnPlayerCantRespawn;

    [SerializeField, Range(0, 100)] private uint numberPlayerRespawn = 3;


    private void Awake()
    {
        flagMaterial = flag.GetComponent<MeshRenderer>().material;
        flagMaterial.color = offColor;
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
            flagMaterial.color = onColor;
            OnCheckPointIsActivate?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        if (!isActivate)
            Gizmos.DrawWireSphere(transform.position, radiusZone);
    }

    public void RespawnPlayer (GameObject player)
    {
        if (numberPlayerRespawn > 0)
        {
            numberPlayerRespawn--;

            if (numberPlayerRespawn == 0)
            {
                OnPlayerCantRespawn?.Invoke();
            }
            else
            {
                player.transform.position = transform.position;
                OnPlayerRespawn?.Invoke();
            }
        }
    }

    public void SetFlag(bool flag)
    {
        if (flag)
        {
            flagMaterial.color = onColor;
            isActivate = true;
        }
        else
        {
            flagMaterial.color = offColor;
            isActivate = false;
        }
    }
}
