using System.Collections.Generic;
using LeopotamGroup.Serialization;
using UnityEngine;

namespace LeopotamGroup.Examples.SerializationTest {
    public class SerializationTest : MonoBehaviour {
        struct StructType {
            public int ID;

            [JsonIgnore]
            public int IgnoredInt;

            public string StructString;

            [JsonName ("s2")]
            public string RenamedString;

            public override string ToString () {
                return string.Format ("[StructType: ID = {0}, IgnoredInt = {1}, StructString = {2}, RenamedString = {3}]",
                    ID, IgnoredInt, StructString ?? "<null>", RenamedString ?? "<null>");
            }
        }

        enum EnumType {
            None,
            Option1,
            Option2
        }

        class TestClass {
            [JsonName ("i")]
            public int ID;

            [JsonName ("e")]
            public EnumType Type;

            [JsonIgnore]
            public string IgnoredString;

            [JsonName ("d")]
            public List<StructType> Data;

            public override string ToString () {
                string data = null;
                if (Data != null) {
                    data = "";
                    foreach (var item in Data) {
                        if (data.Length > 0) {
                            data += ", ";
                        }
                        data += item.ToString ();
                    }
                } else {
                    data = "<null>";
                }
                return string.Format ("[TestClass: ID = {0}, Type = {1}, IgnoredString = {2}, Data = {3}]",
                    ID, Type, IgnoredString ?? "<null>", data);
            }
        }

        void Start () {
            JsonSerializationTest ();
        }

        void JsonSerializationTest () {
            var a = new TestClass
            {
                ID = 123,
                Type = EnumType.Option1,
                IgnoredString = "Ignored string data",
                Data = new List<StructType> ()
            };
            var b = new StructType
            {
                ID = 456,
                IgnoredInt = 789,
                StructString = "Struct string data",
                RenamedString = "Renamed string data"
            };
            a.Data.Add (b);
            Debug.Log ("Data before serialization: " + a);
            var json = JsonSerialization.SerializeStatic (a);
            Debug.Log ("json: " + json);
            var c = JsonSerialization.DeserializeStatic<TestClass> (json);
            Debug.Log ("Data after deserialization: " + c);
        }
    }
}