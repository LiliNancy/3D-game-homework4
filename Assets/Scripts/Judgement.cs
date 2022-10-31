using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
