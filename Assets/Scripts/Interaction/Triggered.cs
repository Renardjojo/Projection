using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Triggered
{
    Trigger TriggerList { get; set; }

    void OnActivated();
    void OnDisabled();
}
