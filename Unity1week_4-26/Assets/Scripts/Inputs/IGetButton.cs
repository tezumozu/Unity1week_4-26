using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Inputs{
    public interface IGetButton{
        IObservable<ButtonKind> pressedIn{get;}
        IObservable<ButtonKind> releasedIn{get;}
    }

    public enum ButtonKind{
        UP,
        DOWN,
        LEFT,
        RIGHT,
        START
    }
}
