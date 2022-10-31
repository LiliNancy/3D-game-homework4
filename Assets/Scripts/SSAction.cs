using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tt1;

namespace tt2{
public class SSAction : ScriptableObject
{
    public bool enable = true;
    public bool destory = false;
    public GameObject gameobject{get;set;}
    public Transform transform{get;set;}
    public ISSActionCallback callback{get;set;}
    // Start is called before the first frame update
    protected SSAction(){}
    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}

}
