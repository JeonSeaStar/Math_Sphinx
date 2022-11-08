using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class CamelQuestion : MonoBehaviour
{
    [Header("WJAPI")]
    public WJAPI scWJAPI;

    [Header("���� ����")]
    public TEXDraw tdr;
    //public TextMeshProUGUI[] texSelection;
    public TEXDraw[] texSelection;

    [Header("��� ����")]
    public Image dialogue_box;

    [Header("�÷��̾�")]
    public GameObject camel_object;
    public GameObject camera_camel;
    public Animator player_anime;
    int move_hash_code = Animator.StringToHash("Move");

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
    public float bg_speed;
    public GameObject bg_image;
    public Vector3 bg_move_position;
    public texttypingeffect texttypingeffect_cs;
    WaitForSeconds wait_time = new WaitForSeconds(1f);

    void Awake()
    {
        texttypingeffect_cs.prog_gametext5(0);

        StartCoroutine(CreateProblem());

        question_info.current_question_count++;

        if (question_info.question_count == 0) // ���߿� ������ ���ο� ���� �����򤷰� ������ 8������ �����ϰ� ����
            question_info.question_count = 5;
    }

    void Update()
    {
        Stop_Camel();
        if (player_anime.GetBool(move_hash_code))
        {
            bg_image.transform.localPosition = Vector3.MoveTowards(bg_image.transform.localPosition, bg_move_position, Time.deltaTime * bg_speed);
        }

        if (Input.GetKeyDown(KeyCode.Space)) { SceneManager.LoadScene("ProblemHistory"); }
    }

    IEnumerator CreateProblem()
    {
        yield return wait_time;

        scWJAPI.MakeQuestion();
        StartCoroutine(Selection_Text_Setting());
        wait_time = new WaitForSeconds(1f);
    }

    IEnumerator Selection_Text_Setting()
    {
        yield return wait_time;
        for (int i = 0; i < texSelection.Length; i++)
            texSelection[i].text = scWJAPI.Answer_Selection[i];

        tdr.text = /*"\\scdd" +*/ "\\centering" + scWJAPI.Problem_Explain;
    }

    public void Click_Answer(int _nIndex)
    {
        ProblemHistoryData.instance.Save_Problem(DateTime.Now.ToString("yyyy�� MM�� dd��"), tdr, texSelection, texSelection[_nIndex]);

        if (Check_Answer(_nIndex))
            Move_Camel();
        else
            Panelty();

        if (question_info.current_question_count < question_info.question_count)
        {
            scWJAPI.OnClick_Ansr(_nIndex);
            question_info.current_question_count++;
        }
        else
        {
            Result();
        }
    }

    bool Check_Answer(int button_num)
    {
        if (texSelection[button_num].text == scWJAPI.Problem_Answer)
        {
            ProblemHistoryData.instance.Check_Correct();
            return true;
        }
        else
            return false;
    }

    void Move_Camel()
    {
        Clear_Answer_Box(true);
        player_anime.SetBool(move_hash_code, true);
        bg_move_position = new Vector3(bg_image.transform.localPosition.x - 1000, bg_image.transform.localPosition.y + 0, bg_image.transform.localPosition.z);
        question_info.current_correct_value++;
    }

    void Stop_Camel()
    {
        if(player_anime.GetBool(move_hash_code) && bg_move_position == bg_image.transform.localPosition)
        {
            player_anime.SetBool(move_hash_code, false);
            StartCoroutine(Selection_Text_Setting());
        }
    }

    void Panelty()
    {
        Clear_Answer_Box(false);
        StartCoroutine(Selection_Text_Setting());
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
        if (question_info.current_correct_value >= question_info.target_correct_value)
        {
            texttypingeffect_cs.text_board.SetActive(true);
            texttypingeffect_cs.prog_gametext3(0);
        }
        else
        {
            texttypingeffect_cs.text_board.SetActive(true);
            texttypingeffect_cs.prog_gametext4(0);
        }
    }

    public void Exit_Camel_Game()
    {
        SceneManager.LoadScene("InGameScene");
    }

    void Change_Main_Scene()
    {

    }
}