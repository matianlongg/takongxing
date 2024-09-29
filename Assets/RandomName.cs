using UnityEngine;
using TMPro;


public class RandomNameGenerator : MonoBehaviour
{
    public TMP_InputField inputField; // 使用TMP_InputField替代普通的InputField

    private string[] prefixes = {
        "青衣", "赤焰", "白虎", "黑夜", "苍狼", "紫霞", "金龙", "银凤", "天鹰", "碧海",
        "云霄", "雷火", "玄武", "寒霜", "青龙", "红莲", "青山", "紫电", "风雷", "苍鹰",
        "蓝海", "金翼", "火狼", "灵狐", "狂风", "紫月", "青霜", "赤血", "黑凤", "金鹰"
    };

    private string[] middles = {
        "风云", "星月", "影舞", "梦雪", "霜寒", "雷霆", "火影", "雪松", "疾风", "雷电",
        "夜雨", "星辰", "寒冰", "天河", "光影", "烈日", "雪影", "风雪", "雨幕", "流光",
        "破晓", "寒夜", "云霞", "冰魂", "星火", "苍穹", "幻影", "玄月", "雷鸣", "流云"
    };

    private string[] suffixes = {
        "侠客", "游者", "法师", "将军", "宗师", "剑客", "道士", "护卫", "刺客", "影者",
        "守卫", "领主", "君主", "神箭", "圣骑", "弓箭手", "御风者", "灵师", "术士", "战士",
        "天师", "玄士", "灵剑", "鬼道", "剑圣", "影舞者", "天狼", "鬼影", "风行者", "毒师"
    };

    public void GenerateNames()
    {
        inputField.text = GenerateInputNames();
    }

    public string GenerateInputNames()
    {
        string names = "";

        int randomPrefixIndex = Random.Range(0, prefixes.Length);
        int randomMiddleIndex = Random.Range(0, middles.Length);
        int randomSuffixIndex = Random.Range(0, suffixes.Length);

        string randomName = prefixes[randomPrefixIndex] + middles[randomMiddleIndex] + suffixes[randomSuffixIndex];
        return randomName;
    }
}
