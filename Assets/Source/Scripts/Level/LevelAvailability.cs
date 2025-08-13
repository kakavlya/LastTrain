
[System.Serializable]
public class LevelAvailability
{
    public string Name;
    public bool Available;

    public LevelAvailability(string name, bool available)
    {
        Name = name;
        Available = available;
    }
}
