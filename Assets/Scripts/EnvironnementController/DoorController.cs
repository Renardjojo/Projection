using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum OpeningType
{
    FromTopToBottom,
    FromLeftToRight,
    FromBottomToTop,
    FromRightToLeft
}

public class DoorController : MonoBehaviour
{
    [SerializeField] OpeningType openingType = OpeningType.FromBottomToTop;
    [SerializeField]
    [Range(0.001f, 1f)]
    [Tooltip("Define lerp speed by second. This value move from 0 to 1. Lerp at 0 is not possible. Lerp at 1 chang immediatly the color")]
    private float lerpSpeed = 0.2f;
    [Range(0f, 1f)]
    [Tooltip("Define the opening between 0 and 1. 1 for the door is not open when opening and 0 for the door is totaly open")]
    private float opening = 1f;

    public void Open()
    {
        gameObject.SetActive(false);
        switch (openingType)
        {
            case OpeningType.FromBottomToTop:
                break;

            case OpeningType.FromLeftToRight:

                break;

            case OpeningType.FromRightToLeft:

                break;

            case OpeningType.FromTopToBottom:

                break;
        }
    }

    public void Close()
    {
        gameObject.SetActive(true);
        switch (openingType)
        {
            case OpeningType.FromBottomToTop:

                break;

            case OpeningType.FromLeftToRight:

                break;

            case OpeningType.FromRightToLeft:

                break;

            case OpeningType.FromTopToBottom:

                break;
        }
    }
}
