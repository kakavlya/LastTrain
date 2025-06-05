using UnityEngine;

public class VisualEffect : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _eventTriggeredObject;
    [SerializeField] private ParticleSystem _particle;

    private IEventTrigger _trigger;

    private void OnEnable()
    {
        if (_eventTriggeredObject is IEventTrigger)
        {
            _trigger = _eventTriggeredObject as IEventTrigger;
            _trigger.OnTriggered += PlayEffect;
        }
        else
        {
            Debug.LogError("MonoBehaviour object is not an IEventTrigger, its type is" +
                _eventTriggeredObject.GetType());
        }
    }

    private void OnDisable()
    {
        if (_eventTriggeredObject is IEventTrigger)
        {
            _trigger.OnTriggered -= PlayEffect;
        }
    }

    private void PlayEffect()
    {
        if (_particle != null)
            ParticlePool.Instance.GetParticle(_particle, _particle.gameObject.transform.position);
    }
}
