using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Player;

namespace GameManagers{
    public class ScoreManager : MonoBehaviour{
        public int totalScore {
            get;
            private set;
        }

        int distance;//距離に関するスコア

        [SerializeField]
        GameObject resultUI; 

        [SerializeField]//距離スコア
        Text distanceNum;

        [SerializeField]//カモスコア
        Text kogamoNum;

        [SerializeField]//NextGame
        Text next;

        [SerializeField]//合計スコア
        Text totalNum;

        // Start is called before the first frame update
        void Start(){
            //初期化
            StatusManager.status
                .Where(_ => _ == GameStatus.INIT)
                .Subscribe(_ => {
                    totalScore = 0;
                }).AddTo(gameObject);
            
            //ゲーム開始準備
            StatusManager.status
                .Where(_ => _ == GameStatus.READY)
                .Subscribe(_ => {
                    distance = 0;
                    //リザルトを非表示
                    resultUI.SetActive(false);
                }).AddTo(gameObject);

            //ゲーム中
            this.UpdateAsObservable()
            .Where(_ => StatusManager.status.Value == GameStatus.GAME)
            .Subscribe(_ => {
                distance = (int)(StageManager.maxTime - StageManager.time)*50;
            });

            //リザルトの表示
            StatusManager.status
                .Where(_ => _ == GameStatus.RESULT)
                .Subscribe(_ => {
                    int kogamo = 0;

                    //距離スコア
                    distanceNum.text = "+ " + distance.ToString();

                    //かもスコア
                    Kamo player = GameObject.Find("Kamo").GetComponent<Kamo>();
                    kogamo = player.child.Value * 100 * StageManager.stageLevel;
                    kogamoNum.text = "+ " + kogamo.ToString();

                    //合計
                    totalScore += distance + kogamo;
                    totalNum.text = totalScore.ToString();

                    //次があるかどうか
                    if(StageManager.stageLevel == 5 || StatusManager.gameEnd){
                        next.text = "FINISH GAME"; 
                    }else{
                        next.text = "NEXT STAGE";
                    }

                    //表示
                    resultUI.SetActive(true);
                }).AddTo(gameObject);
        }
    }
}
