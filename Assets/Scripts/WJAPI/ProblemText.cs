using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TexDrawLib;

public class ProblemText : MonoBehaviour
{
    public Dictionary<string, string> problem_text = new Dictionary<string, string>();

    [System.Serializable]
    public class problem
    {
        
    }

    private void Awake()
    {
        Setting_Dictionary(); //���� ����� �����Ƽ� ����ٰ� ����
        print(problem_text.Count);
    }

    public void Setting_Dictionary()
    {
        problem_text.Add("1 + 3 = \\square", "\\katuri{1 + 3");

        problem_text.Add("\begin{align}  28~\\ -~~~26~\\ \\hline  \\square \\end{align}", "\\katuri{28 - 26");

        problem_text.Add("\begin{align}  38 ~\\  \times ~~~1~  \\ \\hline  \\square\\square \\end{align}", "\\katuri{38 \times 1");

        problem_text.Add("\frac{7}{3}=\\square", "\frac{7}{3}");

        problem_text.Add("\begin{align}  ~ ) \\underline{ ~~ 24 ~~~~ 18 ~}~~~~�ִ�����:~\\square \\  ~~  \\end{align}", "\\katuri{24, 18�� �ִ�����");

        problem_text.Add("4+x=6 ~\rightarrow~ x=\\square", "\\katuri{4+x = 6, x��?");

        problem_text.Add("10\\%~�λ�\rightarrow\\square\\square\\square", "\\katuri{100�� 10% �λ�");

        problem_text.Add("\begin{align} &15a^{4}b^{3}\\div\\left(-5a^{3}b\right)\times\\left(-ab^{2}\right)\\=\\square\\square\\square\\square\\square\\square\\square\\square\\square\\square\\square \\end{align}", "\\katuri{15a^{4}b^{3}\\div\\left(-5a^{3}b\right)\times\\eft(-ab^{2}\right)}");

        problem_text.Add("\begin{align}  38~\\ -~~~13~\\ \\hline  \\square\\square \\end{align}", "\\katuri{38 - 13}");

        problem_text.Add("10\\%~����\rightarrow\\square\\square\\square", "\\katuri{100�� 10% ����}");

        problem_text.Add("2ab\\div\frac{1}{4}a^{3}\times a^{4}b^{2}=\\square\\square\\square\\square\\square\\square\\square\\square\\square\\square\\square", "\\katuri2ab\\div\frac{1}{4}a^{3}\times a^{4}b^{2}");

        problem_text.Add("\frac{8}{5}=\\square", "\\katuri\frac{8}{5}");

        problem_text.Add("\begin{align}  ~ ) \\underline{ ~~ 4 ~~~~ 8 ~} ~~~\rightarrow~�ִ�����:~\\square \\  ~~  \\end{align}", "\\katuri{4, 8�� �ִ�����}");

        problem_text.Add("4+x=7+1 ~\rightarrow~ x=\\square", "\\katuri{4+x=7+1, x��?}");

        problem_text.Add("0����~3��ŭ~ū~��\rightarrow\\square\\square", "\\katuri{0���� 3��ŭ ū ��}");

        problem_text.Add("\begin{align}  25~\\ -~~~15~\\ \\hline  \\square\\square \\end{align}", "\\katuri25 - 15");

        problem_text.Add("\begin{align}  12 ~\\  \times ~~~4~  \\ \\hline  \\square\\square \\end{align}", "\\katuri12 \times 4");

        problem_text.Add("\frac{12}{4}=\\square", "\\katuri\frac{12}{4}");

        problem_text.Add("3 + 3 = \\square", "\\katuri3 + 3");

        problem_text.Add("\begin{align}  39~\\ -~~~23~\\ \\hline  \\square\\square \\end{align}", "\\katuri39 - 23");

        problem_text.Add("4 + 3 = \\square", "\\katuri4 + 3");

        problem_text.Add("2 + 3 = \\square", "\\katuri2 + 3");

        problem_text.Add("\begin{align}  39~\\ -~~~23~\\ \\hline  \\square\\square \\end{align}", "\\katuri2 + 3");

        problem_text.Add("\begin{align}  38~\\ -~~~13~\\ \\hline  \\square\\square \\end{align} ", "\\katuri2 + 3");
    }

    public string Retext_Problems(string s)
    {
        foreach (var item in problem_text)
        {
            AWS.instance.text.text = item.Key;

            if (s == AWS.instance.text.text)
            {
                AWS.instance.text.text = item.Value;
                return AWS.instance.text.text;
            }
        }
        return s;
    }
}
