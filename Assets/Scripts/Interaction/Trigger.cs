using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System;

public interface Trigger
{
    public bool IsOn { get; set; }

    public event Action onTriggered;
    public event Action onUntriggered;
}
