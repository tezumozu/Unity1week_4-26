using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Traps{
    public class CatHand : Trap{
        void OnCollisionEnter2D(Collision2D collision){

            IBeCaught kogamo = collision.gameObject.GetComponent<IBeCaught>();
            if(kogamo == null){
                return;
            }
            kogamo.caught(this.kind,this);
        }
    }
}

