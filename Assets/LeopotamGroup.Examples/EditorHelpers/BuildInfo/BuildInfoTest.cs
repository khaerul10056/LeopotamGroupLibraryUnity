using LeopotamGroup.EditorHelpers;
using UnityEngine;

namespace LeopotamGroup.Examples.EditorHelpers.BuildInfos {
    public class BuildInfoTest : MonoBehaviour {
        void OnGUI () {
            GUILayout.Label ("You cant get app namespace and version at runtime for all platforms with standard unity api!");
            GUILayout.Label ("Application namespace: " + BuildInfo.Instance.AppName);
            GUILayout.Label ("Application version: " + BuildInfo.Instance.AppVersion);
        }
    }
}