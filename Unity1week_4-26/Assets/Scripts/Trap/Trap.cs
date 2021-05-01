using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GameManagers;

namespace Traps{
    public enum TrapKind{
        CAT,
        GUM,
        HOLE
    }

    public class Trap:MonoBehaviour{
        protected TrapKind kind;
        protected float velocity = 0.03f;

        protected float destoryPoint = -8.0f;
        // Start is called before the first frame update
    }
}
