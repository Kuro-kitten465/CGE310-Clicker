public enum SlimeSize
{
    Small,
    Medium,
    Large,
    Huge
}

[System.Serializable]
public class SlimeData
{
    public SlimeSize size;
    public float health;
    public float scale;
    public int juiceReward;
    public float expReward;
}