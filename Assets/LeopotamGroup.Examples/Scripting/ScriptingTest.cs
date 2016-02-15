using LeopotamGroup.Scripting;
using UnityEngine;

namespace LeopotamGroup.Examples.ScriptingTest {
    public class ScriptingTest : MonoBehaviour {
        const string ScriptName = "Scripts/TestScript";

        const string MainFunctionName = "main";

        void Start () {
            var scriptSource = Resources.Load <TextAsset> (ScriptName).text;
            var err = MyScriptManager.Instance.LoadSource (scriptSource);
            if (err != null) {
                // We have error here.
                Debug.LogWarning (err);
                return;
            }
            // Code was loaded and parsed correctly, we ready to call functions

            ScriptVar retVal;
            err = MyScriptManager.Instance.CallFunction (MainFunctionName, out retVal, new ScriptVar (1), new ScriptVar ("str"));
            if (err != null) {
                // We have error here.
                Debug.LogWarning (err);
                return;
            }
        }
    }
}