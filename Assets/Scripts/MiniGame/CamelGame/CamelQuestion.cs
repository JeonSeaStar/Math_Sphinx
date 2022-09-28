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

    [Header("�÷��̾� ������")]
    public int current_life = 3;
    public Image[] life_images = new Image[3];
    public Color[] life_colours = new Color[2];         //���Ŀ� �̹����� ���� ����

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
    }

    IEnumerator CreateProblem()
    {
        yield return new WaitForSeconds(1f);

        question_info.current_question_count++;

        scWJAPI.MakeQuestion();
        StartCoroutine(Selection_Text_Setting());
    }

    IEnumerator Selection_Text_Setting()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < texSelection.Length; i++)
            texSelection[i].text = scWJAPI.Answer_Selection[i];

        tdr.text = scWJAPI.Problem_Explain;
    }

    public void Click_Answer(int _nIndex)
    {
        question_info.current_question_count++;

        if (Check_Answer(_nIndex))
            Move_Camel();
        else
            Panelty();

        scWJAPI.OnClick_Ansr(_nIndex);

        StartCoroutine(Selection_Text_Setting());
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
        print("�����Դϴ�.");

        question_info.current_correct_value++;
    }

    void Panelty()
    {
        print("�����Դϴ�.");

        current_life--;
        life_images[current_life].color = life_colours[1];

        if (current_life == 0)
        {
            print("���� ���");
        }
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