
[System.Serializable]
public class TrainProgress : BaseProgress
{
    public int HealthLevel;
    public int SlotsLevel;

    public TrainProgress(int defaultStatLevel = 0)
    {
        HealthLevel = defaultStatLevel;
        SlotsLevel = defaultStatLevel;
    }

    public override int GetLevel(StatType stat)
    {
        switch (stat)
        {
            case StatType.Health:
                return HealthLevel;

            case StatType.Slots:
                return SlotsLevel;
        }

        return 0;
    }

    public override void Increment(StatType stat)
    {
        switch (stat)
        {
            case StatType.Health:
                HealthLevel++;
                break;

            case StatType.Slots:
                SlotsLevel++;
                break;
        }
    }
}
