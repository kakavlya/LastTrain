using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Core
{
    public interface IPoolable
    {
        public void ReturnToPool();
    }
}