[System.Serializable]
public class ScoreData
{
    public string player_name;
    public float score;

    public string created_at;
    public string updated_at;

    public ScoreData(string playerName, float score)
    {
        this.player_name = playerName;
        this.score = score;
    }
}


[System.Serializable]
public class ScoreDataList
{
    public ScoreData[] scores;
}