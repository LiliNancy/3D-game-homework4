# priests and evils (动作分离版)
## 实现动作分离，并且，设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束
效果如图所示：
![avatar](pic/look.png)
只改变了其实现，所以效果图没变。
### 动作基类（SSAction）
这里直接使用了课件给出的代码，作为基类，由此衍生出来各种各样的动作
代码如下：
```
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
```
其中ISSActionCallback接口作为接收通知对象的抽象类型。
代码如下：
```
public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, 
    SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0, string strParam = null, Object objectParam = null);
}
```
### 简单动作实现（MoveToAction）
此处，实现一些简单动作，在本次作业中因为只需要移动船和角色，故而只有此一个简单动作。
代码如下：
```
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
```
### 顺序动作组合类实现（SequenceAction）
通过该类，把简单动作组合起来，以此实现一些较为复杂的类。
代码如下：
```
public class SequenceAction : SSAction,ISSActionCallback
{
    public List<SSAction> sequence;
    public int repeat = -1;
    public int start = 0;
    public static SequenceAction GetSSAction(int repeat,int start,List<SSAction> sequence){
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }
    public override void Start()
    {
        foreach(SSAction action in sequence){
            action.gameobject = this.gameobject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }
    public override void Update()
    {
        if(sequence.Count == 0) return;
        if(start < sequence.Count){
            sequence[start].Update();
        }
    }
    public void SSActionEvent(SSAction source,SSActionEventType events = SSActionEventType.Competeted,
    int intParam = 0,string strParam = null,Object objectParam = null){
        source.destory = false;
        this.start++;
        if(this.start>=sequence.Count){
            this.start=0;
            if(repeat>0) repeat--;
            if(repeat==0){
                this.destory = true;
                this.callback.SSActionEvent(this);
            }
        }
    }
    void OnDestory(){

    }
}
```
### 动作管理基类（ActionManager）
是动作对象管理器的基类，实现了对所有动作（简单动作、顺序动作等）的基本管理。
代码如下：
```
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
```
### 该游戏的动作管理类（MyManager)
针对本游戏需要，实现了一个动作管理类，用以移动船和角色，且保证了动作发生时玩家无法进行其他操作。
代码如下：
```
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
```
### 裁判类（Judgment）
按照题目要求，把原来在FirstController中实现的check分出来，单独实现。
代码如下：
```
public class Judgement : System.Object
{
    public static Judgement _instance;
    public static Judgement getInstance(){
        if(_instance==null){
            _instance = new Judgement();
        }
        return _instance;
    }
    public string checkres(FirstController fc){
        if(fc.statue!=0) return " ";
        int re=0,rp=0,le=0,lp=0;
        for(int i=0;i<6;i++){
            if(fc.charactor[i].kind==0){
                if(fc.charactor[i].Cwhere==0) rp++;
                else if(fc.charactor[i].Cwhere==-1&&fc.boat.boatWhere==0) rp++;
                else lp++;
            }
            else{
                if(fc.charactor[i].Cwhere==0) re++;
                else if(fc.charactor[i].Cwhere==-1&&fc.boat.boatWhere==0) re++;
                else le++;
            }
        }
        fc.statue=3;
        if(re>rp&&rp!=0) {
            return "Game Over";  
        }
        else if(le>lp&&lp!=0) {
            return "Game Over";
        }
        else if(lp==3&&le==3) {
            return "You Win!";
        }
        fc.statue=0;
        return " ";
    }
}
```

### 结果展示
相较于上一版，这次角色上船下船时有了动作，详细代码见github（Assets/Scripts）。
视频网站：https://live.csdn.net/v/249696

