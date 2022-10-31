using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using test1;
using test2;
using test3;
using test5;
namespace test4{
    public class boatCon{
        public GameObject boatm;
        public int boatWhere;
        public Cha[] roles=new Cha[2];
        public boatCon(GameObject obj){
            boatm=obj;
            boatWhere=0;
        }
    }
}
