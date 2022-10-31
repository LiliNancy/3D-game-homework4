using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tt1;
using tt2;

namespace tt4{
public class MoveToAction : SSAction
{
    public Vector3 target;
    public float speed;
    public static MoveToAction GetSSAction(Vector3 target,float speed){
        MoveToAction action = ScriptableObject.CreateInstance<MoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position,target,speed*Time.deltaTime);
        if(target==this.transform.position){
            this.destory = true;
            this.callback.SSActionEvent(this);
        }
    }
}    
}

