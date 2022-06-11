using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Lean.Common;
using Lean.Touch;

[Serializable]
public class Chess{
    public GameObject General = null;
    public GameObject Guard = null;
    public GameObject Elephant = null;
    public GameObject Rook = null;
    public GameObject Horse = null;
    public GameObject Cannon = null;
    public GameObject Soldier = null;
}

public class ChessMove : MonoBehaviour
{
    public GameObject Point = null;
    public Chess RedChess = null;
    public Chess BlackChess = null;

    private string[] RedChessName = {"SoldierR", "CannonR", "HorseR", "RookR", "ElephantR", "GuardR", "GeneralR"};
    private string[] BlackChessName = {"SoldierB", "CannonB", "HorseB", "RookB", "ElephantB", "GuardB", "GeneralB"};

    private bool chessselect = false;
    private GameObject tmp = null;
    private List<string> actpoints = new List<string>();
    private List<string> history = new List<string>();
    
    //Function InitChess
    public void InitChess(GameObject model, string name, int i, int j)
    {
        tmp = Instantiate(model, new Vector3(0,0,0), Quaternion.identity);
        tmp.name = name;
        tmp.transform.parent = transform.Find(System.Convert.ToString(i) + System.Convert.ToString(j));
        tmp.transform.localPosition = new Vector3(0, 0, 0);
        tmp.transform.localRotation = model.transform.localRotation;
        tmp.transform.localScale = model.transform.localScale;
    }
    //Function InitChessBoard
    public void InitChessBoard()
    { 
        //Destory All
        for(int i=0; i<transform.childCount; i++){
            if(transform.GetChild(i).childCount > 1)
                Destroy(transform.GetChild(i).GetChild(1).gameObject);
        }
        actpoints = new List<string>();
        history = new List<string>();
        GameObject.Find("Button").SendMessage("ResetStep");

        //Init Red Chess
        //Soldier
        InitChess(RedChess.Soldier, RedChessName[0], 6, 0);
        InitChess(RedChess.Soldier, RedChessName[0], 6, 2);
        InitChess(RedChess.Soldier, RedChessName[0], 6, 4);
        InitChess(RedChess.Soldier, RedChessName[0], 6, 6);
        InitChess(RedChess.Soldier, RedChessName[0], 6, 8);
        //Cannon
        InitChess(RedChess.Cannon, RedChessName[1], 7, 1);
        InitChess(RedChess.Cannon, RedChessName[1], 7, 7);
        //Horse
        InitChess(RedChess.Horse, RedChessName[2], 9, 1);
        InitChess(RedChess.Horse, RedChessName[2], 9, 7);
        //Rook
        InitChess(RedChess.Rook, RedChessName[3], 9, 0);
        InitChess(RedChess.Rook, RedChessName[3], 9, 8);
        //Elephant
        InitChess(RedChess.Elephant, RedChessName[4], 9, 2);
        InitChess(RedChess.Elephant, RedChessName[4], 9, 6);
        //Guard
        InitChess(RedChess.Guard, RedChessName[5], 9, 3);
        InitChess(RedChess.Guard, RedChessName[5], 9, 5);
        //General
        InitChess(RedChess.General, RedChessName[6], 9, 4);

        //Init Black Chess
        //Soldier
        InitChess(BlackChess.Soldier, BlackChessName[0], 3, 0);
        InitChess(BlackChess.Soldier, BlackChessName[0], 3, 2);
        InitChess(BlackChess.Soldier, BlackChessName[0], 3, 4);
        InitChess(BlackChess.Soldier, BlackChessName[0], 3, 6);
        InitChess(BlackChess.Soldier, BlackChessName[0], 3, 8);
        //Cannon
        InitChess(BlackChess.Cannon, BlackChessName[1], 2, 1);
        InitChess(BlackChess.Cannon, BlackChessName[1], 2, 7);
        //Horse
        InitChess(BlackChess.Horse, BlackChessName[2], 0, 1);
        InitChess(BlackChess.Horse, BlackChessName[2], 0, 7);
        //Rook
        InitChess(BlackChess.Rook, BlackChessName[3], 0, 0);
        InitChess(BlackChess.Rook, BlackChessName[3], 0, 8);
        //Elephant
        InitChess(BlackChess.Elephant, BlackChessName[4], 0, 2);
        InitChess(BlackChess.Elephant, BlackChessName[4], 0, 6);
        //Guard
        InitChess(BlackChess.Guard, BlackChessName[5], 0, 3);
        InitChess(BlackChess.Guard, BlackChessName[5], 0, 5);
        //General
        InitChess(BlackChess.General, BlackChessName[6], 0, 4);
    }
    //Awake
    void Awake()
    {   
        //Init Dead
        tmp = new GameObject();
        tmp.name = "Dead";
        tmp.transform.parent = transform;
        tmp.SetActive(false);
        //Init Point
        for(int i=0; i<10; i++){
            for(int j=0; j<9; j++){
                tmp = Instantiate(Point, new Vector3(0,0,0), Quaternion.identity);
                tmp.name = System.Convert.ToString(i) + System.Convert.ToString(j);
                tmp.transform.parent = transform;
                tmp.transform.localPosition = new Vector3(j*0.111f, i*0.0829f, 0);
                tmp.transform.localScale = Point.transform.localScale;
                tmp.transform.localRotation = Point.transform.localRotation;
                tmp.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        InitChessBoard();
    }

    //Function ShowPath
    public bool ShowPath(int num, bool Red)
    {
        string str = System.Convert.ToString(num);
        if(num < 10){
            str = "0" + str;
        }

        if(GameObject.Find(str).transform.childCount > 1){
            if(Red){
                if(RedChessName.Contains(GameObject.Find(str).transform.GetChild(1).gameObject.name)){
                    // 讓被吃得棋子做出反應
                    return false;
                }
            }
            else{
                if(BlackChessName.Contains(GameObject.Find(str).transform.GetChild(1).gameObject.name)){
                    // 讓被吃得棋子做出反應
                    return false;
                }
            }
        }
        actpoints.Add(str);
        GameObject.Find(str).transform.GetChild(0).gameObject.SetActive(true);
        return true;
    }
    //Function IsNotStuck
    public bool IsNotStuck(int num)
    {
        string str = System.Convert.ToString(num);
        if(num < 10){
            str = "0" + str;
        }

        if(GameObject.Find(str).transform.childCount > 1){
            return false;
        }
        else{
            return true;
        }
    }
    //ChessOnSelect
    public void ChessOnSelect(string name)
    {
        if(chessselect == true) return;

        chessselect = true;
        tmp = transform.Find(name).gameObject;
        tmp.transform.localPosition = new Vector3(tmp.transform.localPosition.x, tmp.transform.localPosition.y, 0.05f);

        int num = 0;
        int.TryParse(tmp.name, out num);
    //Soldier
        if(tmp.transform.GetChild(1).gameObject.name == RedChessName[0]){
            if(num-10 > 0){
                ShowPath(num-10, true);
            }
            if(num < 50){
                if(num%10 != 0){
                    ShowPath(num-1, true);
                }
                if(num%10 != 8){
                    ShowPath(num+1, true);
                }
            }
        }
        else if(tmp.transform.GetChild(1).gameObject.name == BlackChessName[0]){
            if(num+10 < 99){
                ShowPath(num+10, false);
            }
            if(num >= 50){
                if(num%10 != 0){
                    ShowPath(num-1, false);
                }
                if(num%10 != 8){
                    ShowPath(num+1, false);
                }
            }
        }
    //Cannon
        else if(tmp.transform.GetChild(1).gameObject.name == RedChessName[1]){
            int count = num;
            while(count-10 >= 0){
                count -= 10;
                if(!IsNotStuck(count)){
                    while(count-10 >= 0){
                        count -= 10;
                        if(!IsNotStuck(count)){
                            ShowPath(count, true);
                            break;
                        }
                    }
                    break;
                }
                ShowPath(count, true);
            }
            count = num;
            while(count+10 < 99){
                count += 10;
                if(!IsNotStuck(count)){
                    while(count+10 < 99){
                        count += 10;
                        if(!IsNotStuck(count)){
                            ShowPath(count, true);
                            break;
                        }
                    }
                    break;
                }
                ShowPath(count, true);
            }
            count = num;
            while(count%10 > 0){
                count -= 1;
                if(!IsNotStuck(count)){
                    while(count%10 > 0){
                        count -= 1;
                        if(!IsNotStuck(count)){
                            ShowPath(count, true);
                            break;
                        }
                    }
                    break;
                }
                ShowPath(count, true);
            }
            count = num;
            while(count%10 < 8){
                count += 1;
                if(!IsNotStuck(count)){
                    while(count%10 < 8){
                        count += 1;
                        if(!IsNotStuck(count)){
                            ShowPath(count, true);
                            break;
                        }
                    }
                    break;
                }
                ShowPath(count, true);
            }
        }
        else if(tmp.transform.GetChild(1).gameObject.name == BlackChessName[1]){
            int count = num;
            while(count-10 >= 0){
                count -= 10;
                if(!IsNotStuck(count)){
                    while(count-10 >= 0){
                        count -= 10;
                        if(!IsNotStuck(count)){
                            ShowPath(count, false);
                            break;
                        }
                    }
                    break;
                }
                ShowPath(count, false);
            }
            count = num;
            while(count+10 < 99){
                count += 10;
                if(!IsNotStuck(count)){
                    while(count+10 < 99){
                        count += 10;
                        if(!IsNotStuck(count)){
                            ShowPath(count, false);
                            break;
                        }
                    }
                    break;
                }
                ShowPath(count, false);
            }
            count = num;
            while(count%10 > 0){
                count -= 1;
                if(!IsNotStuck(count)){
                    while(count%10 > 0){
                        count -= 1;
                        if(!IsNotStuck(count)){
                            ShowPath(count, false);
                            break;
                        }
                    }
                    break;
                }
                ShowPath(count, false);
            }
            count = num;
            while(count%10 < 8){
                count += 1;
                if(!IsNotStuck(count)){
                    while(count%10 < 8){
                        count += 1;
                        if(!IsNotStuck(count)){
                            ShowPath(count, false);
                            break;
                        }
                    }
                    break;
                }
                ShowPath(count, false);
            }
        }
    //Horse
        else if(tmp.transform.GetChild(1).gameObject.name == RedChessName[2]){
            if(num-10 >= 0){
                if(num%10 < 8){
                    if(IsNotStuck(num-10) && num-10 >= 10) ShowPath(num-19, true);
                    if(IsNotStuck(num+1) && num%10 < 7) ShowPath(num-8, true);
                }
                if(num%10 > 0){
                    if(IsNotStuck(num-10) && num-10 >= 10) ShowPath(num-21, true);
                    if(IsNotStuck(num-1) && num%10 > 1) ShowPath(num-12, true);
                }
            }
            if(num+10 < 99){
                if(num%10 < 8){
                    if(IsNotStuck(num+10) && num+10 < 89) ShowPath(num+21, true);
                    if(IsNotStuck(num+1) && num%10 < 7) ShowPath(num+12, true);
                }
                if(num%10 > 0){
                    if(IsNotStuck(num+10) && num+10 < 89) ShowPath(num+19, true);
                    if(IsNotStuck(num-1) && num%10 > 1) ShowPath(num+8, true);
                }
            }
        }
        else if(tmp.transform.GetChild(1).gameObject.name == BlackChessName[2]){
            if(num-10 >= 0){
                if(num%10 < 8){
                    if(IsNotStuck(num-10) && num-10 >= 10) ShowPath(num-19, false);
                    if(IsNotStuck(num+1) && num%10 < 7) ShowPath(num-8, false);
                }
                if(num%10 > 0){
                    if(IsNotStuck(num-10) && num-10 >= 10) ShowPath(num-21, false);
                    if(IsNotStuck(num-1) && num%10 > 1) ShowPath(num-12, false);
                }
            }
            if(num+10 < 99){
                if(num%10 < 8){
                    if(IsNotStuck(num+10) && num+10 < 89) ShowPath(num+21, false);
                    if(IsNotStuck(num+1) && num%10 < 7) ShowPath(num+12, false);
                }
                if(num%10 > 0){
                    if(IsNotStuck(num+10) && num+10 < 89) ShowPath(num+19, false);
                    if(IsNotStuck(num-1) && num%10 > 1) ShowPath(num+8, false);
                }
            }
        }
    //Rook
        else if(tmp.transform.GetChild(1).gameObject.name == RedChessName[3]){
            int count = num;
            while(count-10 >= 0 && ShowPath(count-10, true)){
                count -= 10;
                if(!IsNotStuck(count)) break;
            }
            count = num;
            while(count+10 < 99 && ShowPath(count+10, true)){
                count += 10;
                if(!IsNotStuck(count)) break;
            }
            count = num;
            while(count%10 > 0 && ShowPath(count-1, true)){
                count -= 1;
                if(!IsNotStuck(count)) break;
            }
            count = num;
            while(count%10 < 8 && ShowPath(count+1, true)){
                count += 1;
                if(!IsNotStuck(count)) break;
            }
        }
        else if(tmp.transform.GetChild(1).gameObject.name == BlackChessName[3]){
            int count = num;
            while(count-10 >= 0 && ShowPath(count-10, false)){
                count -= 10;
                if(!IsNotStuck(count)) break;
            }
            count = num;
            while(count+10 < 99 && ShowPath(count+10, false)){
                count += 10;
                if(!IsNotStuck(count)) break;
            }
            count = num;
            while(count%10 > 0 && ShowPath(count-1, false)){
                count -= 1;
                if(!IsNotStuck(count)) break;
            }
            count = num;
            while(count%10 < 8 && ShowPath(count+1, false)){
                count += 1;
                if(!IsNotStuck(count)) break;
            }
        }
    //Elephant
        else if(tmp.transform.GetChild(1).gameObject.name == RedChessName[4]){
            if(num-10 >= 50){
                if(num%10 < 8){
                    if(IsNotStuck(num-9)) ShowPath(num-18, true);
                }
                if(num%10 > 0){
                    if(IsNotStuck(num-11)) ShowPath(num-22, true);
                }
            }
            if(num+10 < 99){
                if(num%10 < 8){
                    if(IsNotStuck(num+11)) ShowPath(num+22, true);
                }
                if(num%10 > 0){
                    if(IsNotStuck(num+9)) ShowPath(num+18, true);
                }
            }
        }
        else if(tmp.transform.GetChild(1).gameObject.name == BlackChessName[4]){
            if(num-10 >= 0){
                if(num%10 < 8){
                    if(IsNotStuck(num-9)) ShowPath(num-18, false);
                }
                if(num%10 > 0){
                    if(IsNotStuck(num-11)) ShowPath(num-22, false);
                }
            }
            if(num+10 < 49){
                if(num%10 < 8){
                    if(IsNotStuck(num+11)) ShowPath(num+22, false);
                }
                if(num%10 > 0){
                    if(IsNotStuck(num+9)) ShowPath(num+18, false);
                }
            }
        }
    //Guard
        else if(tmp.transform.GetChild(1).gameObject.name == RedChessName[5]){
            if(num-10 >= 70){
                if(num%10 < 5){
                    ShowPath(num-9, true);
                }
                if(num%10 > 3){
                    ShowPath(num-11, true);
                }
            }
            if(num+10 < 99){
                if(num%10 < 5){
                    ShowPath(num+11, true);
                }
                if(num%10 > 3){
                    ShowPath(num+9, true);
                }
            }
        }
        else if(tmp.transform.GetChild(1).gameObject.name == BlackChessName[5]){
            if(num-10 >= 0){
                if(num%10 < 5){
                    ShowPath(num-9, true);
                }
                if(num%10 > 3){
                    ShowPath(num-11, true);
                }
            }
            if(num+10 < 30){
                if(num%10 < 5){
                    ShowPath(num+11, true);
                }
                if(num%10 > 3){
                    ShowPath(num+9, true);
                }
            }
        }
    //General
        else if(tmp.transform.GetChild(1).gameObject.name == RedChessName[6]){
            if(num-10 >= 70){
                ShowPath(num-10, true);
            }
            if(num+10 < 99){
                ShowPath(num+10, true);
            }
            if(num%10 > 3){
                ShowPath(num-1, true);
            }
            if(num%10 < 5){
                ShowPath(num+1, true);
            }
        }
        else if(tmp.transform.GetChild(1).gameObject.name == BlackChessName[6]){
            if(num-10 >= 0){
                ShowPath(num-10, false);
            }
            if(num+10 < 30){
                ShowPath(num+10, false);
            }
            if(num%10 > 3){
                ShowPath(num-1, false);
            }
            if(num%10 < 5){
                ShowPath(num+1, false);
            }
        }
    }

    //ChessOffSelect
    public void ChessOffSelect()
    {
        for(int i=0; i<actpoints.Count; i++){
            GameObject.Find(actpoints[i]).transform.GetChild(0).gameObject.SetActive(false);
        }
        actpoints = new List<string>();

        tmp.transform.localPosition = new Vector3(tmp.transform.localPosition.x, tmp.transform.localPosition.y, 0);
        GameObject.Find("Chesses").SendMessage("DeselectAll");
        tmp = null;
        chessselect = false;
    }

    //ChessOnMove
    public void ChessOnMove(string name)
    {
        if(chessselect == false) return;

        string str = tmp.name + " " + name + " ";

        Transform tar = transform.Find(name);
        if(tar.childCount > 1){
            str +=  tar.GetChild(1).gameObject.name;
            tar.GetChild(1).SetParent(transform.Find("Dead"), false);
        }
        tmp.transform.GetChild(1).SetParent(transform.Find(name), false);

        history.Add(str);
        GameObject.Find("Button").SendMessage("SetStep", 1);

        ChessOffSelect();
    }

    //Next Step
    public void NextStep(string str)
    {
        string now = str.Substring(0, 2);
        string next = str.Substring(3);
        ChessOnSelect(now);
        ChessOnMove(next);
    }

    //Previous Step
    public void PreviousStep()
    {
        if(history.Count > 0){
            string previous = history[history.Count-1].Substring(0, 2);
            string now = history[history.Count-1].Substring(3, 2);
            string dead = "";
            if(history[history.Count-1].Length > 6){
                dead = history[history.Count-1].Substring(6);
                transform.Find("Dead").Find(dead).SetParent(transform.Find(now), false);
            }
            transform.Find(now).GetChild(1).SetParent(transform.Find(previous), false);

            history.RemoveAt(history.Count-1);
            GameObject.Find("Button").SendMessage("SetStep", -1);
        }
    }
}