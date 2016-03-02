using LeopotamGroup.Protection;
using UnityEngine;

namespace LeopotamGroup.Examples.ProtectionTest {
    public class ProtectionTest : MonoBehaviour {
        void Start () {
            IntTest ();
            LongTest ();
            FloatTest ();
        }

        void IntTest () {
            Debug.Log (">>>>> Test protection of int >>>>>");
            int testValue = 12345678;
            ProtInt protValue = testValue;
            Debug.LogFormat ("{0} encrypted to {1}", testValue, protValue.EncryptedValue);
            Debug.LogFormat ("{0} decrypted to {1}", protValue.EncryptedValue, (int) protValue);
        }

        void LongTest () {
            Debug.Log (">>>>> Test protection of long >>>>>");
            long testValue = 12345678L;
            ProtLong protValue = testValue;
            Debug.LogFormat ("{0} encrypted to {1}", testValue, protValue.EncryptedValue);
            Debug.LogFormat ("{0} decrypted to {1}", protValue.EncryptedValue, (long) protValue);
        }

        void FloatTest () {
            Debug.Log (">>>>> Test protection of float >>>>>");
            float testValue = 1234.5678f;
            ProtFloat protValue = testValue;
            Debug.LogFormat ("{0} encrypted to {1}", testValue, protValue.EncryptedValue);
            Debug.LogFormat ("{0} decrypted to {1}", protValue.EncryptedValue, (float) protValue);
        }
    }
}