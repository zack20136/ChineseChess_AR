using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class ButtonEvent : MonoBehaviour
{
    private string[] RedChessName = {"GeneralR", "GuardR", "ElephantR", "RookR", "HorseR", "CannonR", "SoldierR"};
    private string[] BlackChessName = {"GeneralB", "GuardB", "ElephantB", "RookB", "HorseB", "CannonB", "SoldierB"};
    private string[] RedChess = {"帥", "仕", "相", "俥", "傌", "炮", "兵"};
    private string[] BlackChess = {"將", "士", "象", "車", "馬", "包", "卒"};
    private string[] RedNum = {"一", "二", "三", "四", "五", "六", "七", "八", "九"};
    private string[] BlackNum = {"1", "2", "3", "4", "5", "6", "7", "8", "9"};

    public TMP_Text Pre;
    public TMP_Text Now;
    public TMP_Text Next;

    private List<string> record = new List<string>();
    private int step = 0;
    private int stage = 0;
    private int value = 0;

    //Choose Record
    public void ChooseRecord()
    {
        // GameObject.Find("ChooseRecord").SendMessage("AddOptions", new List<string> {"ADD"}); // 增加選單選項

        GameObject.Find("Chesses").SendMessage("InitChessBoard");
        record = new List<string>();
        Pre.text = "Pre";
        Now.text = "Now";
        Next.text = "Next";

        value = GameObject.Find("ChooseRecord").GetComponent<TMP_Dropdown>().value;
        if(value == 0) return;

        BetterStreamingAssets.Initialize();
        string[] subs = BetterStreamingAssets.ReadAllText(System.Convert.ToString(value) + ".txt").Split('\n');
        foreach (var sub in subs){
            record.Add(sub);
        }
    }

    //Function Get Col
    public int GetCol(string s)
    {
        if(s == RedNum[8] || s == BlackNum[0]){
            return 0;
        }
        else if(s == RedNum[7] || s == BlackNum[1]){
            return 1;
        }
        else if(s == RedNum[6] || s == BlackNum[2]){
            return 2;
        }
        else if(s == RedNum[5] || s == BlackNum[3]){
            return 3;
        }
        else if(s == RedNum[4] || s == BlackNum[4]){
            return 4;
        }
        else if(s == RedNum[3] || s == BlackNum[5]){
            return 5;
        }
        else if(s == RedNum[2] || s == BlackNum[6]){
            return 6;
        }
        else if(s == RedNum[1] || s == BlackNum[7]){
            return 7;
        }
        else if(s == RedNum[0] || s == BlackNum[8]){
            return 8;
        }
        return -1;
    }
    //Function Find RedChess
    public int FindRedChess(int num, int n)
    {
        Transform tmp = GameObject.Find("Chesses").transform;
        for(int i=0; i<10; i++){
            if(tmp.Find(System.Convert.ToString(i) + System.Convert.ToString(n)).childCount > 1){
                if(tmp.Find(System.Convert.ToString(i) + System.Convert.ToString(n)).GetChild(1).name == RedChessName[num]){
                    n = i*10 + n;
                    return n; // 當同一排有兩個相同的棋 可能會出問題
                }
            }
        }
        return -1;
    }
    //Function FindBlackChess
    public int FindBlackChess(int num, int n)
    {
        Transform tmp = GameObject.Find("Chesses").transform;
        for(int i=0; i<10; i++){
            if(tmp.Find(System.Convert.ToString(i) + System.Convert.ToString(n)).childCount > 1){
                if(tmp.Find(System.Convert.ToString(i) + System.Convert.ToString(n)).GetChild(1).name == BlackChessName[num]){
                    n = i*10 + n;
                    return n; // 當同一排有兩個相同的棋 可能會出問題
                }
            }
        }
        return -1;
    }
    //Function Decode
    public string DecodeRecord()
    {   
        string s1 = record[step].Substring(0, 1);
        string s2 = record[step].Substring(1, 1);
        string s3 = record[step].Substring(2, 1);
        string s4 = record[step].Substring(3, 1);

        int n1 = GetCol(s2);
        int n2 = GetCol(s4);

        int now = 0;
        int next = 0;
    //General
        if(s1 == RedChess[0]){
            now = FindRedChess(0, n1);
            if(s3 == "平"){
                next = now + (n2-n1);
            }
            else if(s3 == "進"){
                next = now - 10*(9-n2);
            }
            else if(s3 == "退"){
                next = now + 10*(9-n2);
            }
        }
        else if(s1 == BlackChess[0]){
            now = FindBlackChess(0, n1);
            if(s3 == "平"){
                next = now + (n2-n1);
            }
            else if(s3 == "進"){
                next = now + 10*(n2+1);
            }
            else if(s3 == "退"){
                next = now - 10*(n2+1);
            }
        }
    //Guard
        else if(s1 == RedChess[1]){
            now = FindRedChess(1, n1);
            if(s3 == "進"){
                next = now - (10 + n1-n2);
            }
            else if(s3 == "退"){
                next = now + (10 + n2-n1);
            }
        }
        else if(s1 == BlackChess[1]){
            now = FindBlackChess(1, n1);
            if(s3 == "進"){
                next = now + (10 + n2-n1);
            }
            else if(s3 == "退"){
                next = now - (10 + n1-n2);
            }
        }
    //Elephant
        else if(s1 == RedChess[2]){
            now = FindRedChess(2, n1);
            if(s3 == "進"){
                next = now - (20 + n1-n2);
            }
            else if(s3 == "退"){
                next = now + (20 + n2-n1);
            }
        }
        else if(s1 == BlackChess[2]){
            now = FindBlackChess(2, n1);
            if(s3 == "進"){
                next = now + (20 + n2-n1);
            }
            else if(s3 == "退"){
                next = now - (20 + n1-n2);
            }
        }
    //Rook
        else if(s1 == RedChess[3]){
            now = FindRedChess(3, n1);
            if(s3 == "平"){
                next = now + (n2-n1);
            }
            else if(s3 == "進"){
                next = now - 10*(9-n2);
            }
            else if(s3 == "退"){
                next = now + 10*(9-n2);
            }
        }
        else if(s1 == BlackChess[3]){
            now = FindBlackChess(3, n1);
            if(s3 == "平"){
                next = now + (n2-n1);
            }
            else if(s3 == "進"){
                next = now + 10*(n2+1);
            }
            else if(s3 == "退"){
                next = now - 10*(n2+1);
            }
        }
    //Horse
        else if(s1 == RedChess[4]){
            now = FindRedChess(4, n1);
            if(s3 == "進"){
                if(Mathf.Abs(n1-n2) == 1){
                    next = now - (20 + n1-n2);
                }
                else{
                    next = now - (10 + n1-n2);
                }
            }
            else if(s3 == "退"){
                if(Mathf.Abs(n1-n2) == 1){
                    next = now + (20 + n2-n1);
                }
                else{
                    next = now + (10 + n2-n1);
                }
            }
        }
        else if(s1 == BlackChess[4]){
            now = FindBlackChess(4, n1);
            if(s3 == "進"){
                if(Mathf.Abs(n2-n1) == 1){
                    next = now + (20 + n2-n1);
                }
                else{
                    next = now + (10 + n2-n1);
                }
            }
            else if(s3 == "退"){
                if(Mathf.Abs(n1-n2) == 1){
                    next = now - (20 + n1-n2);
                }
                else{
                    next = now - (10 + n1-n2);
                }
            }
        }
    //Cannon
        else if(s1 == RedChess[5]){
            now = FindRedChess(5, n1);
            if(s3 == "平"){
                next = now + (n2-n1);
            }
            else if(s3 == "進"){
                next = now - 10*(9-n2);
            }
            else if(s3 == "退"){
                next = now + 10*(9-n2);
            }
        }
        else if(s1 == BlackChess[5]){
            now = FindBlackChess(5, n1);
            if(s3 == "平"){
                next = now + (n2-n1);
            }
            else if(s3 == "進"){
                next = now + 10*(n2+1);
            }
            else if(s3 == "退"){
                next = now - 10*(n2+1);
            }
        }
    //Soldier
        else if(s1 == RedChess[6]){
            now = FindRedChess(6, n1);
            if(s3 == "平"){
                next = now + (n2-n1);
            }
            else if(s3 == "進"){
                next = now - 10*(9-n2);
            }
        }
        else if(s1 == BlackChess[6]){
            now = FindBlackChess(6, n1);
            if(s3 == "平"){
                next = now + (n2-n1);
            }
            else if(s3 == "進"){
                next = now + 10*(n2+1);
            }
        }

        string str = "";
        if(now >= 10 && next >= 10){
            str = System.Convert.ToString(now) + " " + System.Convert.ToString(next);
        }
        else if(now < 10 && next >= 10){
            str = "0" + System.Convert.ToString(now) + " " + System.Convert.ToString(next);
        }
        else if(now >= 10 && next < 10){
            str = System.Convert.ToString(now) + " 0" + System.Convert.ToString(next);
        }
        else{
            str = "0" + System.Convert.ToString(now) + " 0" + System.Convert.ToString(next);
        }

        return str;
    }
    //Next Step
    public void NextStep()
    {
        if(step < stage) stage = step;
        if(step == stage){
            if(record.Count > step){
                string str = DecodeRecord();
                if(step-1 >= 0) Pre.text = record[step-1];
                if(step >= 0) Now.text = record[step];
                else Now.text = "NULL";
                if(record.Count > step+1) Next.text = record[step+1];
                else Next.text = "NULL";
                stage += 1;
                GameObject.Find("Chesses").SendMessage("NextStep", str);
            }
        }
    }

    //Previous Step
    public void PreviousStep()
    {
        if(step == stage){
            if(step > 2) Pre.text = record[step-3];
            else Pre.text = "NULL";
            if(step > 1) Now.text = record[step-2];
            else Now.text = "NULL";
        }
        if(step > stage) Next.text = "X";
        else if(step > 0) Next.text = record[step-1];
        if(step == stage && stage > 0) stage -= 1;
        GameObject.Find("Chesses").SendMessage("PreviousStep");
    }

    public void ResetStep(){
        step = 0;
        stage = 0;
    }

    public void SetStep(int n)
    {
        step += n;
        if(step > stage) Next.text = "X";
        else if(record.Count > step && step >= 0) Next.text = record[step];
    }
}
