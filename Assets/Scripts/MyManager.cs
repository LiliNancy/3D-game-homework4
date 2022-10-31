using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tt1;
using tt2;
using tt3;
using tt4;
using test2;

public class MyManager : ActionManager,ISSActionCallback
{
    public SSAction MoveBoat;
    public SequenceAction MoveCha;
    public FirstController fc; 
    // Start is called before the first frame update
    protected void Start()
    {
        fc = SSDirector.getInstance().currentSceneController as FirstController;
        fc.actioncontroller = this;
    }
    public void Moveboat(GameObject b,Vector3 target,int speed){
        MoveBoat = MoveToAction.GetSSAction(target,speed);
        this.RunAction(b,MoveBoat,this);
    }
    public void Movecha(GameObject b,Vector3 target,Vector3 midtarget,int speed){
        SSAction ac1 = MoveToAction.GetSSAction(midtarget,speed);
        SSAction ac2 = MoveToAction.GetSSAction(target,speed);
        MoveCha = SequenceAction.GetSSAction(1,0,new List<SSAction>{ac1,ac2});
        this.RunAction(b,MoveCha,this);
    }
    // Update is called once per frame
    protected new void Update()
    {
        if(fc.statue == 1 && ((MoveToAction)MoveBoat).target==MoveBoat.gameobject.transform.position) fc.statue = 0; 
        if(fc.statue == 2 && MoveCha.repeat==0) fc.statue = 0;
        base.Update();
    }
    public void SSActionEvent(SSAction source, 
    SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0, string strParam = null, Object objectParam = null){

    }
}
