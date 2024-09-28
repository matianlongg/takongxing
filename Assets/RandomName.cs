using UnityEngine;
using TMPro;


public class RandomNameGenerator : MonoBehaviour
{
    public TMP_InputField inputField; // 使用TMP_InputField替代普通的InputField

    private string[] prefixes = { "青衣", "赤焰", "白虎", "黑夜", "苍狼", "紫霞", "金龙", "银凤" };
    private string[] middles = { "风云", "星月", "影舞", "梦雪", "霜寒", "雷霆", "火影", "雪松" };
    private string[] suffixes = { "侠客", "游者", "法师", "将军", "宗师", "剑客", "道士", "护卫" };

    public void GenerateNames()
    {
        inputField.text = GenerateInputNames();
    }

    public string GenerateInputNames()
    {
        string names = "";

        for (int i = 0; i < 6; i++)
        {
            int randomPrefixIndex = Random.Range(0, prefixes.Length);
            int randomMiddleIndex = Random.Range(0, middles.Length);
            int randomSuffixIndex = Random.Range(0, suffixes.Length);

            string randomName = prefixes[randomPrefixIndex] + middles[randomMiddleIndex] + suffixes[randomSuffixIndex];
            names += randomName;

            if (i < 5)
            {
                names += "\n";
            }
        }

        return names;
    }
}
