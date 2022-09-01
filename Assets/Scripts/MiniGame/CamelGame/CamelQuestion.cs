using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CamelQuestion : MonoBehaviour
{
    // �츮�� �ʿ��Ѱ� 
    public string Problem_Answer;          // ������ �������� api�� ���� �˾ƿ� ����
    public string[] Answer_Selection;      // 4������ ���� ���� ������
    public int Answer_num;                 // �� 4�� �迭�� �����° �������� ������.

    public WJ_Conn scWJ_Conn;
    public TextMeshProUGUI txQuestion;

    public TextMeshProUGUI[] txAnsr;


    public enum STATE
    {
        DN_SET,
        DN_PROG,
        LEARNING,
    }

    public STATE eState;
    protected bool bRequest;

    protected int nDigonstic_Idx;

    protected WJ_Conn.Learning_Data cLearning;
    protected int nLearning_Idx;
    protected string[] strQstCransr = new string[8];
    protected long[] nQstDelayTime = new long[8];


    void Awake()
    {
        eState = STATE.LEARNING;              // ���� �ܰ� �����ϴ� �� 

        cLearning = null;
        nLearning_Idx = 0;
        bRequest = false;
        scWJ_Conn.OnRequest_DN_Setting(0);              // 0 ���� ������ ���� 

    }


    public void MakeQuestion()                          // ���� ���� �Լ� 
    {
        switch (eState)
        {
            case STATE.DN_SET: DoDN_Start(); break;             // ������ �������ΰ�� 
            case STATE.LEARNING: DoLearning(); break;           // �н����� ��� 
        }
    }

    // ��ư���� ����� �� [ ���� Ŭ�������� ����� ��]
    public void OnClick_Ansr(int _nIndex)               // ���� ��ư ������
    {
        switch (eState)
        {
            case STATE.DN_SET:
            case STATE.DN_PROG:                         // ���� 
                {
                    DoDN_Prog(Answer_Selection[_nIndex]);
                }
                break;
            case STATE.LEARNING:
                {
                    strQstCransr[nLearning_Idx - 1] = Answer_Selection[_nIndex];
                    nQstDelayTime[nLearning_Idx - 1] = 5000;
                    DoLearning();
                }
                break;
        }
    }


    protected void DoDN_Start()
    {
        nDigonstic_Idx = 0;
        scWJ_Conn.OnRequest_DN_Setting(0);
        bRequest = true;
    }


    protected void DoDN_Prog(string _qstCransr)
    {
        string strYN = "N";
        if (scWJ_Conn.cDiagnotics.data.qstCransr.CompareTo(_qstCransr) == 0)
            strYN = "Y";

        scWJ_Conn.OnRequest_DN_Progress("W",
                                         scWJ_Conn.cDiagnotics.data.qstCd,          // ���� �ڵ�
                                         _qstCransr,                                // ������ �䳻�� -> ����ڰ� ������ ���� ������ �Է�
                                         strYN,                                     // ���俩��("Y"/"N")
                                         scWJ_Conn.cDiagnotics.data.sid,            // ���� SID
                                         5000);                                     // �ӽð� - ���� Ǯ�̿� �ҿ�� �ð�

        bRequest = true;
    }


    protected void DoLearning()
    {
        if (cLearning == null)
        {
            nLearning_Idx = 0;

            scWJ_Conn.OnRequest_Learning();

            bRequest = true;
        }
        else
        {
            if (nLearning_Idx >= scWJ_Conn.cLearning_Info.data.qsts.Count)
            {
                scWJ_Conn.OnLearningResult(cLearning, strQstCransr, nQstDelayTime);      // �н� ��� ó��
                cLearning = null;

                return;
            }

            MakeQuestion(cLearning.qsts[nLearning_Idx].qstCn, cLearning.qsts[nLearning_Idx].qstCransr, cLearning.qsts[nLearning_Idx].qstWransr);


            ++nLearning_Idx;

            bRequest = false;
        }
    }


    protected void MakeQuestion(string _qstCn, string _qstCransr, string _qstWransr)
    {
        char[] SEP = { ',' };
        string[] tmWrAnswer;

        txQuestion.text = scWJ_Conn.GetLatexCode(_qstCn);  // <- ���� ������ ���ؽ� ��ȯ �ʿ�

        Problem_Answer = _qstCransr;                      // ���� ����          
        tmWrAnswer = _qstWransr.Split(SEP, System.StringSplitOptions.None);   // ���� ������ ������ �޾��ִ� �ڵ�  , ������ ���ø����� 
        for (int i = 0; i < tmWrAnswer.Length; ++i)
            tmWrAnswer[i] = scWJ_Conn.GetLatexCode(tmWrAnswer[i]);



        int nWrCount = tmWrAnswer.Length;
        //if (nWrCount >= 2)       // 5�������� �̻��� ������ 4�����ٷ� ������
        nWrCount = 1;


        int nAnsrCount = nWrCount + 1;      // ���� ����

        // ���� ����Ʈ�� ������ ����.
        int nAnsridx = UnityEngine.Random.Range(0, nAnsrCount);        // ���� �ε���! �������� ��ġ
        for (int i = 0, q = 0; i < nAnsrCount; ++i, ++q)
        {
            if (i == nAnsridx)
            {
                Answer_Selection[i] = Problem_Answer;
                Answer_num = i;
                --q;
            }
            else
                Answer_Selection[i] = tmWrAnswer[q];
        }


    }


    void Update()
    {
        if (bRequest == true &&
           scWJ_Conn.CheckState_Request() == 1)
        {
            switch (eState)
            {
                case STATE.DN_SET:
                    {
                        MakeQuestion(scWJ_Conn.cDiagnotics.data.qstCn, scWJ_Conn.cDiagnotics.data.qstCransr, scWJ_Conn.cDiagnotics.data.qstWransr);

                        ++nDigonstic_Idx;

                        eState = STATE.DN_PROG;
                    }
                    break;
                case STATE.DN_PROG:
                    {
                        if (scWJ_Conn.cDiagnotics.data.prgsCd == "E")
                        {
                            //   SetActive_Question(false);

                            nDigonstic_Idx = 0;

                            eState = STATE.LEARNING;            // ���� �н� �Ϸ�
                        }
                        else
                        {
                            MakeQuestion(scWJ_Conn.cDiagnotics.data.qstCn, scWJ_Conn.cDiagnotics.data.qstCransr, scWJ_Conn.cDiagnotics.data.qstWransr);

                            ++nDigonstic_Idx;
                        }
                    }
                    break;
                case STATE.LEARNING:
                    {
                        cLearning = scWJ_Conn.cLearning_Info.data;
                        MakeQuestion(cLearning.qsts[nLearning_Idx].qstCn, cLearning.qsts[nLearning_Idx].qstCransr, cLearning.qsts[nLearning_Idx].qstWransr);

                        ++nLearning_Idx;
                    }
                    break;
            }
            bRequest = false;
        }

    }
}
