//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LeopotamGroup.Common;
using LeopotamGroup.Serialization.JsonInternal;

namespace LeopotamGroup.Serialization {
    /// <summary>
    /// Helper for custom naming of fields on json serialization / deserialization.
    /// Useful for decreasing json-data length.
    /// </summary>
    [AttributeUsage (AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class JsonNameAttribute : Attribute {
        /// <summary>
        /// Default initialization.
        /// </summary>
        public JsonNameAttribute () {
        }

        /// <summary>
        /// Initialization with specified name.
        /// </summary>
        /// <param name="name">Field name at json-data.</param>
        public JsonNameAttribute (string name) {
            Name = name;
        }

        /// <summary>
        /// Get json-data based name for field.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }
    }

    /// <summary>
    /// Helper for fields that should be ignored during json serialization / deserialization.
    /// </summary>
    [AttributeUsage (AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class JsonIgnoreAttribute : Attribute {
    }

    /// <summary>
    /// Json serialization.
    /// </summary>
    public sealed class JsonSerialization {
        static readonly JsonSerialization _instance = new JsonSerialization ();

        Scanner _scanner;

        Parser _parser;

        readonly StringBuilder _sb = new StringBuilder (1024);

        readonly HashSet<Type> _numericTypes = new HashSet<Type>
        {
            typeof (byte), typeof (sbyte), typeof (short), typeof (ushort), typeof (int), typeof (uint),
            typeof (long), typeof (ulong), typeof (float), typeof (double)
        };

        /// <summary>
        /// Default initialization.
        /// </summary>
        public JsonSerialization () {
            _scanner = new Scanner ();
            _parser = new Parser (_scanner);
        }

        void SerializeMember (object obj) {
            if (obj == null) {
                return;
            }
            // string
            var asStr = obj as string;
            if (asStr != null) {
                _sb.Append ('"');
                if (asStr.IndexOf ('"') != -1) {
                    asStr = asStr.Replace ("\\", "\\\\");
                    asStr = asStr.Replace ("\"", "\\\"");
                }
                _sb.Append (asStr);
                _sb.Append ('"');
                return;
            }

            var objType = obj.GetType ();

            // nullable
            // null value will be skipped, dont use overrided default values for nullable types.
//            var nullableType = Nullable.GetUnderlyingType (objType);
//            if (nullableType != null) {
//                _sb.Append ("null");
//                return;
//            }

            // number
            if (_numericTypes.Contains (objType)) {
                _sb.Append (((float) Convert.ChangeType (obj, typeof (float))).ToNormalizedString ());
                return;
            }
            if (objType.IsEnum) {
                _sb.Append ((int) obj);
                return;
            }
            // array
            var list = obj as IList;
            if (list != null) {
                _sb.Append ("[");
                var iMax = list.Count;
                if (iMax > 0) {
                    SerializeMember (list[0]);
                }
                for (var i = 1; i < iMax; i++) {
                    _sb.Append (",");
                    SerializeMember (list[i]);
                }
                _sb.Append ("]");
                return;
            }
            // dict
            var dict = obj as IDictionary;
            if (dict != null) {
                var dictEnum = dict.GetEnumerator ();
                var isComma = false;
                bool noNeedWrapKey;
                _sb.Append ("{");
                while (dictEnum.MoveNext ()) {
                    noNeedWrapKey = dictEnum.Key is string;
                    if (isComma) {
                        _sb.Append (",");
                    }
                    if (!noNeedWrapKey) {
                        _sb.Append ("\"");
                    }
                    SerializeMember (dictEnum.Key);
                    if (!noNeedWrapKey) {
                        _sb.Append ("\"");
                    }
                    _sb.Append (":");
                    SerializeMember (dictEnum.Value);
                    isComma = true;
                }
                _sb.Append ("}");
                return;
            }

            // object
            var desc = TypesCache.Instance.GetCache (objType);
            if (desc != null) {
                var isComma = false;
                object val;
                _sb.Append ("{");
                foreach (var field in desc.Fields) {
                    val = field.Value.GetValue (obj);
                    if (val != null) {
                        if (isComma) {
                            _sb.Append (",");
                        }
                        _sb.Append (field.Key);
                        _sb.Append (":");
                        SerializeMember (val);
                        isComma = true;
                    }
                }
                foreach (var prop in desc.Properties) {
                    val = prop.Value.GetValue (obj, null);
                    if (val != null) {
                        if (isComma) {
                            _sb.Append (",");
                        }
                        _sb.Append (prop.Key);
                        _sb.Append (":");
                        SerializeMember (val);
                        isComma = true;
                    }
                }
                _sb.Append ("}");
            }
        }

        /// <summary>
        /// Serialize specified object to json-data.
        /// </summary>
        /// <returns>Json data string.</returns>
        /// <param name="obj">Object to serialize.</param>
        public string Serialize (object obj) {
            if (obj == null) {
                throw new Exception ("instance is null");
            }
            _sb.Length = 0;

            SerializeMember (obj);

            return _sb.ToString ();
        }

        /// <summary>
        /// Deserialize json to instance of strong-typed class.
        /// </summary>
        /// <returns>Deserialized instance.</returns>
        /// <param name="json">Json data.</param>
        /// <typeparam name="T">Type of instance for deserialization.</typeparam>
        public T Deserialize<T> (string json) {
            if (json == null) {
                throw new Exception ("empty json data");
            }
            _scanner.Load (json);
            _parser.SetType (typeof (T));
            var err = _parser.Parse ();
            if (err != null) {
                throw new Exception (err);
            }
            return (T) _parser.Result;
        }

        /// <summary>
        /// Serialize specified object to json-data with singleton json serializator.
        /// </summary>
        /// <returns>Json data string.</returns>
        /// <param name="obj">Object to serialize.</param>
        public static string SerializeStatic (object obj) {
            return _instance.Serialize (obj);
        }

        /// <summary>
        /// Deserialize json to instance of strong-typed class with singleton json serializator.
        /// </summary>
        /// <returns>Deserialized instance.</returns>
        /// <param name="json">Json data.</param>
        /// <typeparam name="T">Type of instance for deserialization.</typeparam>
        public static T DeserializeStatic<T> (string json) {
            return _instance.Deserialize<T> (json);
        }
    }
}