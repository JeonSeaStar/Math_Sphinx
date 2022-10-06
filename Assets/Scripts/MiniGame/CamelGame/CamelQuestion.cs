using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CamelQuestion : MonoBehaviour
{
    [Header("WJAPI")]
    public WJAPI scWJAPI;

    [Header("���� ����")]
    public TEXDraw tdr;
    public TextMeshProUGUI[] texSelection;

    [Header("��� ����")]
    public Image dialogue_box;

    [Header("�÷��̾�")]
    public GameObject camel_object;
    public GameObject camera_camel;

    [System.Serializable] public struct Question_Info
    {
        [Header("���� ��")] public int question_count;
        [Header("���� ���� ��")] public int current_question_count;
        [Header("��ǥ ���� ��")] public int target_correct_value;
        [Header("���� ���� ��")] public int current_correct_value;
    }

    [Header("���� ����")]
    public Question_Info question_info;
    [TextArea] public string[] camel_dialogue = new string[2]; // 0: ��ǥġ �޼� �� 1: ��ǥġ �޼� ���� ��
    public GameObject reward_item;

    void Awake()
    {
        StartCoroutine(CreateProblem());

        question_info.current_question_count++;

        if (question_info.question_count == 0) // ���߿� ������ ���ο� ���� �����򤷰� ������ 8������ �����ϰ� ����
            question_info.question_count = 5;
    }

    IEnumerator CreateProblem()
    {
        yield return new WaitForSeconds(1f);

        scWJAPI.MakeQuestion();
        StartCoroutine(Selection_Text_Setting());
    }

    IEnumerator Selection_Text_Setting()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < texSelection.Length; i++)
            texSelection[i].text = scWJAPI.Answer_Selection[i];

        tdr.text = "\\scdd" + scWJAPI.Problem_Explain;
    }

    public void Click_Answer(int _nIndex)
    {
        if (Check_Answer(_nIndex))
            Move_Camel();
        else
            Panelty();

        if (question_info.current_question_count < question_info.question_count)
        {
            scWJAPI.OnClick_Ansr(_nIndex);
            question_info.current_question_count++;

            StartCoroutine(Selection_Text_Setting());
        }
        else
        {

        }
    }

    bool Check_Answer(int button_num)
    {
        if (texSelection[button_num].text == scWJAPI.Problem_Answer)
            return true;
        else
            return false;
    }

    void Move_Camel()
    {
        Clear_Answer_Box(true);
        question_info.current_correct_value++;
    }

    void Panelty()
    {
        Clear_Answer_Box(false);
    }

    void Clear_Answer_Box(bool b)
    {
        for (int i = 0; i < texSelection.Length; i++)
            texSelection[i].text = "";

        if (b) { tdr.text = "\\scdd �����̿��� !"; }
        else { tdr.text = "\\scdd Ʋ�Ⱦ��...!"; }
    }

    void Result()
    {
        //���� ������ ����
        //���ΰ������� ���ư��� ��ư Ȱ��
    }

    void Change_Main_Scene()
    {

    }
}