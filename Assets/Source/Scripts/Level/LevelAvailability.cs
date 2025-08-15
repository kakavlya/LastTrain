
[System.Serializable]
public class LevelAvailability
{
    public string Name;
    public bool IsAvailable;

    public LevelAvailability(string name, bool isAvailable)
    {
        Name = name;
        IsAvailable = isAvailable;
    }
}
