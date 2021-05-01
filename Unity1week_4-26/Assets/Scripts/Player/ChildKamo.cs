using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Traps;
using GameManagers;

namespace Player{
    interface IBeCaught{
        void caught(TrapKind kind,Trap trap);
    }
    public class ChildKamo : MonoBehaviour,IBeCaught{
        Kamo parent;
        Vector3 velocity = Vector3.zero;
        float rand_x;
        float rand_y;
        bool caughtFlag = false;
        Trap caughtTrap;
        Vector3 caughtPos;

        Subject<Unit> iBeCaught = new Subject<Unit>();
        public IObservable<Unit> beCaught{
            get{
                return iBeCaught;
            }
        }
        

    // Start is called before the first frame update
        void Start(){
            caughtFlag = false;
            //移動
            this.UpdateAsObservable()
                .Where(_ => StatusManager.status.Value == GameStatus.GAME)
                .Where(_ => {return !caughtFlag;})
                .Subscribe(_ => {
                    move();
                });

            //捕まった時
            this.UpdateAsObservable()
                .Where(_ => {return caughtFlag;})
                .Subscribe(_ => {
                    Vector3 nextPos = caughtTrap.transform.position;
                    nextPos.x -= caughtPos.x;
                    nextPos.y -= caughtPos.y;
                    nextPos.z -= caughtPos.z;
                    this.transform.position = nextPos;
                    if(this.transform.position.y < -7.0f){
                        Destroy(gameObject);
                    }
                });

            StatusManager.status
                .Where(status => status == GameStatus.RESET)
                .Subscribe(_ => {
                    iBeCaught.OnCompleted();
                    Destroy(gameObject);
                })
                .AddTo(gameObject);
        }

        public void caught(TrapKind kind,Trap trap){
            iBeCaught.OnNext(Unit.Default);
            iBeCaught.OnCompleted();
            caughtFlag = true;
            caughtTrap = trap;
            caughtPos = new Vector3(trap.transform.position.x-this.transform.position.x, trap.transform.position.y-this.transform.position.y, +1.0f);
        }

        public void setParent(Kamo kamo,float rand_x,float rand_y){
            parent = kamo;
            this.rand_x = rand_x;
            this.rand_y = rand_y;
        }

        void move(){
            Vector3 nextPos = parent.transform.position;
            Vector3 pos = this.transform.position;
            pos = Vector3.SmoothDamp(pos,nextPos,ref velocity,1.0f);

            //はみ出しチェック
            float limit = 4.5f;
            if(pos.y > limit){
                pos.y = limit;
            }else if(pos.y < -limit){
                pos.y = -limit;

            }
            if(pos.x > limit){
                pos.x = limit;
            }else if(pos.x < -limit){
                pos.x = -limit;
            }

            this.transform.position = pos;
        }
    }
}
