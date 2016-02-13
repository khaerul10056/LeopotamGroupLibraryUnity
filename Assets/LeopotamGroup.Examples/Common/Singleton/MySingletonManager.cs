using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Examples.Common.Singleton {
    public class MySingletonManager : UnitySingleton<MySingletonManager> {
        [SerializeField]
        string _stringParameter = "String param value";

        protected override void OnConstruct () {
            // Use this override method instead of Awake, it will guarantee correct initialization for one instance.
            base.OnConstruct ();
            Debug.Log ("MySingletonManager instance created");
            Debug.Log ("MySingletonManager.StringParameter on start: " + _stringParameter);
        }

        protected override void OnDestruct () {
            // Use this override method instead of OnDestroy, it will guarantee correct dispose for one instance.

            // Dont forget to check UnitySingleton<T>.IsInstanceCreated () at any OnDestroy method (it can be
            // already killed before), otherwise new instance will be created and unity throw exception.

            Debug.Log ("MySingletonManager instance destroyed");
            base.OnDestruct ();
        }

        public void Test () {
            Debug.Log ("MySingletonManager.Test method called");
        }

        public string GetStringParameter () {
            return _stringParameter;
        }
    }
}