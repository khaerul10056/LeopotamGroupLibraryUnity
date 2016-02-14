using LeopotamGroup.Scripting;
using UnityEngine;

namespace LeopotamGroup.Examples.ScriptingTest {
    class MyScriptManager : ScriptManagerBase<MyScriptManager> {
        protected override void OnAttachHostFunctions (ScriptVM vm) {
            base.OnAttachHostFunctions (vm);
            // Registering our custom methods for access from scripts.
            vm.RegisterHostFunction ("test_sqrt", OnSqrt);
            vm.RegisterHostFunction ("test_echo", OnTest);
        }

        protected override void OnRuntimeError (string errMsg) {
            Debug.LogError ("Script error: " + errMsg);
            base.OnRuntimeError (errMsg);
        }

        ScriptVar OnSqrt (ScriptVM vm) {
            var count = vm.GetParamsCount ();
            var v = vm.GetParamByID (0);
            if (count < 1 || !v.IsNumber) {
                vm.SetRuntimeError ("(nValue) parameter required");
                return new ScriptVar ();
            }
            return new ScriptVar (Mathf.Sqrt (v.AsNumber));
        }

        ScriptVar OnTest (ScriptVM vm) {
            var count = vm.GetParamsCount ();
            if (count != 1) {
                vm.SetRuntimeError ("test_echo function requires only one parameter");
                return new ScriptVar ();
            }
            var v = vm.GetParamByID (0);
            Debug.Log ("OnTest callback called with parameter: " + v.AsString);
            return v;
        }
    }
}