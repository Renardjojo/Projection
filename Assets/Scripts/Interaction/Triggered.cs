using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Triggered
{
    public Trigger TriggerList { get; set; }

    public void OnActivated();
    public void OnDisabled();
}
