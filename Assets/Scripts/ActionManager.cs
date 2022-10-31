using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tt1;
using tt2;
using tt4;

public class ActionManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<int,SSAction> actions = new Dictionary<int,SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    protected void Update()
    {
        foreach(SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;
        waitingAdd.Clear();
        foreach(KeyValuePair<int,SSAction> kv in actions){
            SSAction ac = kv.Value;
            if(ac.destory){
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if(ac.enable){
                ac.Update();
            }
        }
        foreach(int key in waitingDelete){
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }
    public void RunAction(GameObject gameObject,SSAction action,ISSActionCallback icb){
        action.gameobject = gameObject;
        action.transform = gameObject.transform;
        action.callback = icb;
        waitingAdd.Add(action);
        action.Start();
    }

    protected void Start()
    {  
    }
}
