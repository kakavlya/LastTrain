using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private int _maxCountProjectiles;

    private int _currentCountProjectiles;

    public bool HaveProjectiles { get; private set; } = true;


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
            HaveProjectiles = false;
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
            HaveProjectiles = true;
        }
    }
}
