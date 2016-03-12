using System.Collections;
using LeopotamGroup.Math;
using LeopotamGroup.Pooling;
using UnityEngine;

namespace LeopotamGroup.Examples.PoolingTest {
    public class PoolingTest : MonoBehaviour {
        [SerializeField]
        PoolContainer _pool;

        const float SpawnDelay = 0.2f;

        IEnumerator Start () {
            if (_pool == null) {
                yield break;
            }
            var waiter = new WaitForSeconds (SpawnDelay);
            PoolObject obj;
            while (true) {
                obj = _pool.Get ();
                obj.transform.localPosition = new Vector3 (
                    Mathf.Lerp (-1f, 1f, Rng.GetFloatStatic ()), Mathf.Lerp (-1f, 1f, Rng.GetFloatStatic ()), 0f);
                obj.transform.localRotation =
                    Quaternion.Euler (new Vector3 (0f, Mathf.Lerp (-180f, 1f, Rng.GetFloatStatic ()), 0f));
                obj.SetActive (true);
                yield return waiter;
            }
        }

        void OnGUI () {
            GUILayout.Label (string.Format (
                "New instances will be spawned from code each {0}secs, recycled each 1secs and reused again without create new instances", SpawnDelay));
        }
    }
}