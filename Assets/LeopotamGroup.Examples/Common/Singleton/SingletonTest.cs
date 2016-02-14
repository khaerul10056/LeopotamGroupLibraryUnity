using UnityEngine;

namespace LeopotamGroup.Examples.Common.SingletonTest {
    public class SingletonTest : MonoBehaviour {
        void Start () {
            MySingletonManager.Instance.Test ();
            Debug.Log ("MySingletonManager.GetStringParameter: " + MySingletonManager.Instance.GetStringParameter ());
        }

        void OnDestroy () {
            // Dont forget to check UnitySingleton<T>.IsInstanceCreated () at any OnDestroy method (it can be
            // already killed before, execution order not defined), otherwise new instance of singleton class
            // will be created and unity throw exception about it.
            if (MySingletonManager.IsInstanceCreated ()) {
                Debug.Log ("MySingletonManager still alive!");
            } else {
                Debug.Log ("MySingletonManager already killed!");
            }
        }
    }
}