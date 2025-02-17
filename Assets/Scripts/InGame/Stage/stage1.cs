    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class stage1 : MonoBehaviour
{
    bool trigger = false;
    bool trigger2 = false;
    // UI - key
    public bool is_key1 = false;
    public bool is_key2 = false;

    public GameObject UI_key1;
    public GameObject UI_key2;

    public Sprite nonekey;
    public Sprite iskey;

    //
    public GameObject Endpopup;
    public GameObject Endpopup_retryBTN;
    public GameObject Endpopup_yesBTN;
    public Image endpopup_retryimg;
    public Image endpopup_yesimg;

    public TextMeshProUGUI goldtext;
    public TextMeshProUGUI scoretext;

    public GameObject character_prefebs;
    public GameObject character_parent;

    public GameObject[] startgameanswertrigger;
    public GameObject stargame_gb;

    public GameObject[] spears;
    public GameObject map_parent;
    public GameObject key_prefebs;
    public GameObject signboard_prefebs;
    public TEXDraw3D[] first_game_text;
    public GameObject first_textbox;

    public TEXDraw3D[] final_game_text;
    public FindAnswerWay findAnswerWay_cs;
    public InGameManeger inGameManeger_cs;

    public GameObject treasurebox;
    public Sprite treasurebox_sprite;

    // -65, 66 (180  | -59, 52,(-30 | -56.9, 61 (180
    // -95.26 74 .88  2.1   | -62 86 2.1   |   -59.3, 74.2 , 2.1               | -38.8  , 33.6 2 

    //public void stage1

    public void SetKey_UI()
    {
        if (!is_key1)
            UI_key1.GetComponent<Image>().sprite = nonekey;
        else
            UI_key1.GetComponent<Image>().sprite = iskey;
        if (!is_key2)
            UI_key2.GetComponent<Image>().sprite = nonekey;
        else
            UI_key2.GetComponent<Image>().sprite = iskey;

    }


    public void selected_answer(int answer)
    {
        if (InGameManeger.ingamestate == InGameState._4selectgame)
        {
            for (int i = 0; i < 4; i++)
            {
                Destroy(startgameanswertrigger[i]);
            }
        }
        else if (InGameManeger.ingamestate == InGameState.finalareaing)
        {
            bool _clear = findAnswerWay_cs.AnswerCheck(answer);
            findAnswerWay_cs.PlayerSelectAnswer2(answer);

            if (_clear == true)
            {
                findAnswerWay_cs.ShowProblempopup(false);
                InGameManeger.ingamestate = InGameState.problemclear;
                 for (int i = 0; i < 12; i++)
                {
                    Destroy(spears[i]);
                }
            }   
        }
    }
    public void ShowClearpopup()
    {
        StartCoroutine(ClearPopup());
    }

    public void stage1_createproblem()
    {
        findAnswerWay_cs.CreateProblem();
        trigger = true;
    }
    public void stage2_createproblem()
    {
        findAnswerWay_cs.SetAnswerCreateProblem(3);
        trigger2 = true;
    }

    public void secondgamesettings()
    {
        for (int i = 0; i < 4; i++)
        {
            first_game_text[i].text = findAnswerWay_cs.getselection_text(i);
        }
    }
    public void createCharacter()
    {
        Endpopup.SetActive(false);
        inGameManeger_cs.character = Instantiate(character_prefebs, character_parent.transform);
        inGameManeger_cs.character.GetComponent<Character_Collider>().handlightsystem_cs = inGameManeger_cs.handlightsystem_cs;
        inGameManeger_cs.character.GetComponent<Character_Collider>().findAnswerWay_cs = inGameManeger_cs.findanswerway_cs;
        inGameManeger_cs.character.GetComponent<Character_Collider>().stage1_cs = inGameManeger_cs.stage1_cs;
        inGameManeger_cs.character.GetComponent<Character_Collider>().mapcreate_cs = inGameManeger_cs.mapcreater_cs;
        inGameManeger_cs.character.GetComponent<Character_Collider>().loadpirordata_cs = inGameManeger_cs.Loadpirordata_cs;
        inGameManeger_cs.character.GetComponentInChildren<Character_trigger>()._stage1cs = inGameManeger_cs.stage1_cs;
        inGameManeger_cs.character_animator_cs = inGameManeger_cs.character.GetComponent<Character_Animator>();
        inGameManeger_cs.character_move_cs = inGameManeger_cs.character.GetComponent<Character_move>();
        inGameManeger_cs.camera_move_cs.character = inGameManeger_cs.character;
        inGameManeger_cs.JoystickScripts_cs.cm = inGameManeger_cs.character_move_cs;
        inGameManeger_cs.Loadpirordata_cs.character = inGameManeger_cs.character;
        inGameManeger_cs.texttypingeffect_cs.character_move_cs = inGameManeger_cs.character_move_cs;

    }
    public void open_treasurebox()
    {
        treasurebox.GetComponent<SpriteRenderer>().sprite = treasurebox_sprite;
    }
    public void final_game_settgings()
    {
        stage1_createproblem();
        for (int i = 0; i < 4; i++)
        {
            final_game_text[i].text = findAnswerWay_cs.getselection_text(i);
        }
    }
    public void first_game_setttings()
    {
        key_prefebs = Instantiate(key_prefebs);
        key_prefebs.transform.tag = "key";
        for (int i = 0; i < 4; i++)
        {
             first_game_text[i].text = findAnswerWay_cs.getselection_text(i);
        }

        switch (WJAPI.Answer_num)
        {
            case 0:
                key_prefebs.transform.localPosition = new Vector3(-95.26f, 74.88f, 2.1f);
                break;
            case 1:
                key_prefebs.transform.localPosition = new Vector3(-62f, 86f, 2.1f);
                break;
            case 2:
                key_prefebs.transform.localPosition = new Vector3(-59.3f, 74.2f, 2.1f);
                break;
            case 3:
                key_prefebs.transform.localPosition = new Vector3(-47f, 37.6f, 2.1f);
                break;
        }
    }
    void Update()
    {
        if (trigger == true)
        {
            if (findAnswerWay_cs.makeProblemProgress == MakeProblemProgress.finish)
            {
                first_game_setttings();
                trigger = false;
                findAnswerWay_cs.makeProblemProgress = MakeProblemProgress.none;
            }
        }
        if (trigger2 == true)
        {
            if (findAnswerWay_cs.makeProblemProgress == MakeProblemProgress.finish)
            {
                secondgamesettings();
                trigger2 = false;
                findAnswerWay_cs.makeProblemProgress = MakeProblemProgress.none;
            }
        }
    }

    IEnumerator ClearPopup()
    {
        yield return new WaitForSeconds(1f);

        Endpopup.SetActive(true);
        Endpopup.transform.localPosition = new Vector3(0, 2000, 0);

        // 텍스트 바뀌는거

        int goldo = PlayerPrefs.GetInt("Money") + 500;
        int scoreo = PlayerPrefs.GetInt("Score") + 7;

        goldtext.text = goldo.ToString();
        scoretext.text = scoreo.ToString();

        PlayerPrefs.SetInt("Money", goldo);
        PlayerPrefs.SetInt("Score", scoreo);


        for (int i =20; i < 100; i++)
        {
            if (Endpopup.transform.localPosition.y < 20)
            {
                Endpopup.transform.localPosition = new Vector3(0, 0, 0);
                break;
            }
            Endpopup.transform.localPosition = new Vector3(0, 2000- Mathf.Pow(1.12f,i) *8, 0);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1f);


        


        /// 버튼 생기는거
        Endpopup_yesBTN.SetActive(true);

        Color color2 = endpopup_yesimg.color;
        for (float i = 0.2f; i <= 1; i += 0.1f)
        {
            color2.a = i;
            endpopup_yesimg.color = color2;
            yield return new WaitForSeconds(0.06f);
        }
        yield return new WaitForSeconds(0.5f);
        Endpopup_retryBTN.SetActive(true);

        Color color1 = endpopup_retryimg.color;
        for (float i = 0.2f; i <= 1; i+=0.1f)
        {
            color1.a = i;
            endpopup_retryimg.color = color1;
            yield return new WaitForSeconds(0.06f);
        }

    }
}
