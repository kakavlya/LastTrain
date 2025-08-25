
[System.Serializable]
public class LevelAvailability
{
    public int LevelNumber;
    public bool IsAvailable;

    public LevelAvailability(int number, bool isAvailable)
    {
        LevelNumber = number;
        IsAvailable = isAvailable;
    }
}
