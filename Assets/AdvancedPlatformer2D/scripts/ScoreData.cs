[System.Serializable]
public class ScoreData
{
    public string player_name;
    public int score;

    public ScoreData(string playerName, int score)
    {
        this.player_name = playerName;
        this.score = score;
    }
}
