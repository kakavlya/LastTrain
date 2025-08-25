using UnityEngine;
using YG;
using YG.Utils.LB;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private ProgressHandler _progressHandler;
    [SerializeField] private string _techniñalName;

    private void OnEnable()
    {
        _progressHandler.LevelChanged += GetLeaderboard;
        YG2.onGetLeaderboard += CompareScores;
    }

    private void OnDisable()
    {
        _progressHandler.LevelChanged -= GetLeaderboard;
        YG2.onGetLeaderboard -= CompareScores;
    }

    public void GetLeaderboard()
    {
        YG2.GetLeaderboard(_techniñalName);
    }

    private void CompareScores(LBData lBData)
    {
        if (lBData.currentPlayer == null)
        {
            YG2.SetLeaderboard(_techniñalName, _progressHandler.Level);
            return;
        }

        var bestPlayerScore = lBData.currentPlayer.score;

        if (_progressHandler.Level > bestPlayerScore)
        {
            YG2.SetLeaderboard(_techniñalName, _progressHandler.Level);
        }
    }
}
