using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SimpleCalcu : MonoBehaviour
{
    public static double sum;
    public static string Comm;
    private int Sure;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void OnGUI()
    {
        GUI.Box(new Rect(210,25,200,250),"");
        if(GUI.Button(new Rect(360,75,50,25),"AC")) Init();
        if(GUI.Button(new Rect(360,100,50,25),"BACK")) removeEnd();
        if(GUI.Button(new Rect(360,225,50,50),"SURE")) {
            sum = CalculateNum(Comm);
            ShowResult(sum);
        }
        if(GUI.Button(new Rect(210,25,200,50),Comm));
            for(int i=1;i<=9;i++){
                if(GUI.Button(new Rect(210+(i-1)%3*50,25+((i-1)/3+1)*50,50,50),i.ToString())){
                    addS(i.ToString());
                }
            }
            if(GUI.Button(new Rect(260,225,50,50),"0")){
                addS("0");
            }
            if(GUI.Button(new Rect(210,225,50,50),"+")){
                addS("+");
            }
            if(GUI.Button(new Rect(310,225,50,50),"-")){
                addS("-");
            }
            if(GUI.Button(new Rect(360,125,50,50),"*")){
                addS("*");
            }
            if(GUI.Button(new Rect(360,175,50,50),"/")){
                addS("/");
            }
    }

    void Init(){
        sum = 0;
        Comm = " ";
        Sure = 0;
    }
    void addS(string a){
        if(Sure!=1){
                if(Comm.Length>0&&char.IsDigit(Comm[Comm.Length-1])){
                    if(char.IsDigit(a[0]))
                        Comm = Comm + a;
                    else
                        Comm = Comm + " " + a + " ";
                }
                else{
                    if(char.IsDigit(a[0]))
                        Comm = Comm + a;
                    else
                        Comm = Comm + a +" ";
                }     
        }  
    }
    void removeEnd(){
        if(Sure == 1){
            Comm = " ";
            Sure = 0;
        } 
        if(Comm.Length>=1)
            Comm = Comm.Substring(0,Comm.Length - 2);
    }
    void ShowResult(double a){
        Comm = a.ToString()+" ";
        Sure = 1;
    }
    double CalculateNum(string a){
        Stack num = new Stack();
		double k = 0,zhf=1;
		for(int i=0;i<a.Length;i++){
			if(char.IsDigit(a[i])) k = k*10+a[i]-'0';
			else if(a[i] == ' '){
				if(i>0&&char.IsDigit(a[i-1])){
					num.Push(k*zhf);
					k=0;
					zhf = 1;
				}
			}
			else{
				if(a[i] == '-'){
					zhf = -1;
				}
				else if(a[i] == '*' || a[i] =='/'){
					double zan = (double)num.Peek();
					char ww = a[i];
					num.Pop();
					i+=2;
					for(;i<a.Length;i++){
						if(a[i]==' ') break;
						else k = k*10+a[i]-'0';
					}
					if(ww == '*') zan = zan * k;
					else zan = zan / k;
					if(i<a.Length) {
						num.Push(zan);
						k=0;
					}
					else {
						k = zan;
						zhf = 1;
					}
				}
			} 
		}
		k *=zhf;
		while(num.Count>0){
			double aa = (double)num.Peek();
			num.Pop();
			k = k+aa;
		}
		return k;
    }
}
