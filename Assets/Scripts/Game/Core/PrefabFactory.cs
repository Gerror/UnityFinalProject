using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class PrefabFactory
    {
        private DiContainer _container;

        [Inject]
        public PrefabFactory(DiContainer container)
        {
            _container = container;
        }

        public GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity, null);
        }
        
        public GameObject Spawn(GameObject prefab, Transform parent)
        {
            return Spawn(prefab, parent.position, parent.rotation, parent);
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            return _container.InstantiatePrefab(prefab, position, rotation, parent);
        }
    }
}