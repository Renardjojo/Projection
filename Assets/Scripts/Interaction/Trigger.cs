using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System;

public interface Trigger
{
    bool IsOn { get; set; }

    event Action onTriggered;
    event Action onUntriggered;
}
