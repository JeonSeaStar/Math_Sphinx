using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TexDrawLib;

public class RugMathProblem : MonoBehaviour
{
    public WJ_Conn scWJ_Conn;
    public TextMeshProUGUI txQuestion;
    public GameObject rug_prefab;
    public Rug[] btAnsr = new Rug[2];
    public Vector3 spawn_position;
    public RugPlayer player;
    public int current_life = 3;
    public Image[] life_images = new Image[3];
    public Color[] life_color = new Color[2];

    //public GameObject btStart;

    public TextMeshProUGUI[] txAnsr;



    public enum STATE
    {
        DN_SET,         // ������ �����ؾ� �ϴ� �ܰ�
        DN_PROG,        // ������ ������
        LEARNING,       // �н� ������
    }

    public STATE eState;
    protected bool bRequest;

    protected int nDigonstic_Idx;   // ������ �ε���

    protected WJ_Conn.Learning_Data cLearning;
    protected int nLearning_Idx;     // Learning ���� �ε���
    protected string[] strQstCransr = new string[8];        // ����ڰ� ���⿡�� ������ �� ����
    protected long[] nQstDelayTime = new long[8];           // Ǯ�̿� �ҿ�� �ð�

    


    // Start is called before the first frame update
    void Awake()
    {
        eState = STATE.DN_SET;
        //goPopup_Level_Choice.active = false;

        cLearning = null;
        nLearning_Idx = 0;

        txAnsr = new TextMeshProUGUI[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)
            txAnsr[i] = btAnsr[i].answer;

        SetActive_Question(false);
        //btStart.gameObject.active = true;

        bRequest = false;
        current_life = 3;
        OnClick_Level(0);
    }

    void Life_()
    {
        --current_life;
        life_images[current_life].color = life_color[1];

        if (current_life == 0)
        {
            print("���� ���");
        }
    }

    delegate void Penalty_();
    Penalty_ Penalty;

    void Rug_Penalty()
    {
        Debug.Log("�� ���� Ʋ���� �Ӹ�");
    }

    void Comparison_Answers(string ansrCwYn) //������ Ȯ�� �� ���� �� �г�Ƽ
    {
        if (ansrCwYn == "N")
        {
            Penalty = Rug_Penalty;
            Life_();
            Penalty();
        }
    }


    // ���� ���� ��ư Ŭ���� ȣ��
    public void OnClick_MakeQuestion()
    {
        switch (eState)
        {
            case STATE.DN_SET: DoDN_Start(); break;
            //ȣ�� �ȵ�. case STATE.DN_PROG: DoDN_Prog(); break;
            case STATE.LEARNING: DoLearning(); break;
        }
    }




    // �н� ���� ���� �˾����� ����ڰ� ���ؿ� �´� �н��� ���ý� ȣ��
    public void OnClick_Level(int _nLevel)
    {
        nDigonstic_Idx = 0;
        SetActive_Question(true);

        // ���� ��û
        scWJ_Conn.OnRequest_DN_Setting(_nLevel);

        bRequest = true;
    }


    // ���� ����
    public void OnClick_Ansr(int _nIndex)
    {
        switch (eState)
        {
            case STATE.DN_SET:
            case STATE.DN_PROG:
                {
                    // �������� ����
                    DoDN_Prog(txAnsr[_nIndex].text);
                    Spawn_Rug();
                }
                break;
            case STATE.LEARNING:
                {
                    // ������ ������ ������
                    strQstCransr[nLearning_Idx - 1] = txAnsr[_nIndex].text;
                    nQstDelayTime[nLearning_Idx - 1] = 5000;        // �ӽð�
                    // �������� ����
                    DoLearning();
                }
                break;
        }
    }





    protected void DoDN_Start()
    {
        //goPopup_Level_Choice.active = true;
    }


    protected void DoDN_Prog(string _qstCransr)
    {
        string strYN = "N";
        if (scWJ_Conn.cDiagnotics.data.qstCransr.CompareTo(_qstCransr) == 0)      // �ϴ� �ּ�
            strYN = "Y";
        Comparison_Answers(strYN); //���� �߰���

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
            SetActive_Question(true);

            scWJ_Conn.OnRequest_Learning();

            bRequest = true;
        }
        else
        {
            if (nLearning_Idx >= scWJ_Conn.cLearning_Info.data.qsts.Count)
            {
                scWJ_Conn.OnLearningResult(cLearning, strQstCransr, nQstDelayTime);      // �н� ��� ó��
                cLearning = null;

                SetActive_Question(false);
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

        txQuestion.text = scWJ_Conn.GetLatexCode(_qstCn);// ���� ���

        string strAnswer = _qstCransr;  // ���� ������ �޸𸮿� �־��                
        tmWrAnswer = _qstWransr.Split(SEP, System.StringSplitOptions.None);   // ���� ����Ʈ
        for (int i = 0; i < tmWrAnswer.Length; ++i)
            tmWrAnswer[i] = scWJ_Conn.GetLatexCode(tmWrAnswer[i]);



        int nWrCount = tmWrAnswer.Length;
        if (nWrCount >= 2)       // 5�������� �̻��� ������ 4�����ٷ� ������
            nWrCount = 1;


        int nAnsrCount = nWrCount + 1;       // ���� ����
        for (int i = 0; i < btAnsr.Length; ++i)
        {
            if (i < nAnsrCount)
                btAnsr[i].gameObject.active = true;
            else
                btAnsr[i].gameObject.active = false;
        }


        // ���� ����Ʈ�� ������ ����.
        int nAnsridx = UnityEngine.Random.Range(0, nAnsrCount);        // ���� �ε���! �������� ��ġ
        for (int i = 0, q = 0; i < nAnsrCount; ++i, ++q)
        {
            if (i == nAnsridx)
            {
                txAnsr[i].text = strAnswer;
                --q;
            }
            else
                txAnsr[i].text = tmWrAnswer[q];
        }
    }




    protected void SetActive_Question(bool _bActive)
    {
        txQuestion.text = "";
        for (int i = 0; i < btAnsr.Length; ++i)
            btAnsr[i].gameObject.active = _bActive;
    }

    void Spawn_Rug()
    {
        txAnsr = new TextMeshProUGUI[btAnsr.Length];

        GameObject rugs = Instantiate(rug_prefab, player.transform.position + spawn_position, Quaternion.identity);
        for (int i = 0; i < btAnsr.Length; i++)
        {
            btAnsr[i] = rugs.transform.GetChild(i).GetComponent<Rug>();
            btAnsr[i].rmp = this;
            btAnsr[i].player = player;

            txAnsr[i] = btAnsr[i].answer;
        }
    }


    // Update is called once per frame
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
                            SetActive_Question(false);

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
