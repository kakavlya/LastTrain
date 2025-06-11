using UnityEngine;

public class Ammunition : MonoBehaviour
{
    [SerializeField] private int _maxCountProjectiles;

    private int _currentCountProjectiles;

    public bool HasProjectiles { get; private set; } = true;


    private void Start()
    {
        _currentCountProjectiles = _maxCountProjectiles;
    }

    public void DecreaseProjectilesCount()
    {
        if (_currentCountProjectiles > 0)
        {
            _currentCountProjectiles--;
        }
        else
        {
            HasProjectiles = false;
        }

        Debug.Log("Projectile count " +  _currentCountProjectiles);
    }

    public void IncreaseProjectilesCount(int count)
    {
        if (_currentCountProjectiles + count < _maxCountProjectiles)
        {
            _currentCountProjectiles += count;
        }
        else
        {
            _currentCountProjectiles = _maxCountProjectiles;
        }

        if (_currentCountProjectiles > 0)
        {
            HasProjectiles = true;
        }
    }
}
