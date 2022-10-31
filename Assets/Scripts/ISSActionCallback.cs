using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tt2;

namespace tt1{
public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, 
    SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0, string strParam = null, Object objectParam = null);
}
}

