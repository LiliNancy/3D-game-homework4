using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using test2;
using test3;
public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    private Judgement ju;
    private string gameMessage=" ";
    GUIStyle style,bigstyle;
    private float time=60;
    private int st=1;
    private int timego=1;
    // Start is called before the first frame update
    void Start()
    {
        action = SSDirector.getInstance().currentSceneController as IUserAction;
        ju = Judgement.getInstance();

        style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 70;

        bigstyle = new GUIStyle();
        bigstyle.normal.textColor = Color.black;
        bigstyle.fontSize = 20;
    }
    void OnGUI(){
        GUI.Box(new Rect(10,10,120,250), "Menu");
        if(GUI.Button(new Rect(20,40,100,20), "Restart"))
        {
            action.Restart();
            gameMessage = " ";
            if(((FirstController)action).statue!=1&&((FirstController)action).statue!=2){
                time = 60;
                st=1;
            } 
        }
        if(GUI.Button(new Rect(20,70,100,20), "Stop/Continue"))
        {
            timego=1-timego;
            st = timego;
        }
        if(GUI.Button(new Rect(20,100,100,20), "Priests On"))
        {
           if(st==1) action.ChooseCha(0);
        }
        if(GUI.Button(new Rect(20,130,100,20), "Evils On"))
        {
           if(st==1) action.ChooseCha(1);
        }
        if(GUI.Button(new Rect(20,160,100,20), "Move"))
        {
           if(st==1) action.MoveBoat();
        }
        if(GUI.Button(new Rect(20,190,100,20), "Priests Down"))
        {
            if(st==1) action.RemoveCha(0);
        }
        if(GUI.Button(new Rect(20,220,100,20), "Evils Down"))
        {
            if(st==1) action.RemoveCha(1);
        }
        GUI.Label(new Rect(370, 200, 180, 200), gameMessage,style);
        GUI.Label(new Rect(Screen.width - 150,10,100,50), "Time: " + time, bigstyle);
    }
    void Update(){
        string a= ju.checkres((FirstController)action);
        //action.check();
        if(a!=" ")gameMessage = a;
        if(gameMessage==" "&&timego==1){
            time-=Time.deltaTime;
            if(time==0){
                gameMessage = "Game Over";
                st=0;
            } 
        }
    }
}
