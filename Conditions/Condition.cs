using UnityEngine;
using System.Collections;
using System;

public abstract class Condition : CacheBehaviour {

    public event Action<bool> conditionCheckedEvent;

    public bool CheckCondition () {
        return CheckCondition(null);
    }

    public bool CheckCondition(System.Object o) {
        bool returnVal = OnCheckCondition(o);
        if (conditionCheckedEvent != null) {
            conditionCheckedEvent(returnVal);
        }
        return returnVal;
    }

    public abstract bool OnCheckCondition (System.Object o);

}