namespace LastTrain.Persistence
{
    [System.Serializable]
    public abstract class BaseProgress
    {
        public abstract int GetLevel(StatType stat);

        public abstract int GetSumLevels();

        public abstract void Increment(StatType stat);
    }
}
