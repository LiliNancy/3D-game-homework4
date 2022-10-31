using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test5{
    public class Cha{
        public GameObject cha;
        public int num;
        public int kind;//priest:0,evil:1
        public int Cwhere;//右：0，左：1，船上：-1
        public Cha(GameObject obj,int b,int k){
            cha=obj;
            num=b;
            kind=k;
            Cwhere=0;
        }
    }
}