using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ListTest : MonoBehaviour
{
    List<int> intList = new List<int>();
    List<string> strList = new List<string>();

    public static string intToString(int num)
    {
        return num.ToString();
    }

    private void Start()
    {
        intList = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

        strList = intList.ConvertAll(x => x.ToString());

        for (int i = 0; i < strList.Count; i++)
        {
            Debug.Log($"{strList[i]}");
        }
    }

    private void ListMethod()
    {
        intList.RemoveAll(x => x < 3 && x != 0);
    }

    private void LinqMethod()
    {
        intList.Take(intList.Count -1);

        string removeWhiteSpaceResult = String.Concat(strList[0].Where(c => !char.IsWhiteSpace(c))); 
        // strList[0] ���ڿ��� Char�� ������ ������ �ƴѰ�� ��ȯ�� false�� �������� ã�Ƽ�.
        // Contact �޼ҵ�� ��ġ�� => ��������

    }
}
