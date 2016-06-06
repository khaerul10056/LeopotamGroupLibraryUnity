//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LeopotamGroup.Common;

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
    public partial class JsonSerialization {
        static readonly JsonSerialization _instance = new JsonSerialization ();

        Reader _reader;

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
            _reader = new Reader ();
        }

        void SerializeMember (object obj) {
            if (obj == null) {
                return;
            }
            // string
            var asStr = obj as string;
            if (asStr != null) {
                _sb.Append ("\"");
                if (asStr.IndexOf ('"') != -1) {
                    asStr = asStr.Replace ("\\", "\\\\");
                    asStr = asStr.Replace ("\"", "\\\"");
                }
                _sb.Append (asStr);
                _sb.Append ("\"");
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
                _sb.Append (Convert.ChangeType (obj, typeof (int)));
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
                        _sb.Append ("\"");
                        _sb.Append (field.Key);
                        _sb.Append ("\"");
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
            _reader.SetType (typeof (T));
            _reader.SetJson (json);
            return (T) _reader.ParseValue ();
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

        class Reader {
            const string WordStoppers = "{}[],:\"";

            static bool IsWordBreak (char c) {
                return Char.IsWhiteSpace (c) || WordStoppers.IndexOf (c) != -1;
            }

            enum JsonToken {
                None,
                CurlyOpen,
                CurlyClose,
                SquaredOpen,
                SquaredClose,
                Colon,
                Comma,
                String,
                Number,
                True,
                False,
                Null
            }

            readonly Stack<ArrayList> _arrayPool = new Stack<ArrayList> (16);

            readonly char[] _hexBuf = new char[4];

            readonly StringBuilder _stringBuf = new StringBuilder (128);

            string _json;

            int _jsonPos;

            Type _type;

            ArrayList GetArrayItem () {
                if (_arrayPool.Count > 0) {
                    return _arrayPool.Pop ();
                }
                return new ArrayList ();
            }

            void RecycleArrayItem (ArrayList item) {
                if (item != null) {
                    item.Clear ();
                    _arrayPool.Push (item);
                }
            }

            int jsonPeek () {
                return _jsonPos < _json.Length ? _json[_jsonPos] : -1;
            }

            char jsonRead () {
                return _json[_jsonPos++];
            }

            object ParseByToken (JsonToken token) {
                switch (token) {
                    case JsonToken.String:
                        return ParseString ();
                    case JsonToken.Number:
                        return ParseNumber ();
                    case JsonToken.CurlyOpen:
                        return ParseObject ();
                    case JsonToken.SquaredOpen:
                        return ParseArray ();
                    case JsonToken.True:
                        return true;
                    case JsonToken.False:
                        return false;
                    case JsonToken.Null:
                        return null;
                    default:
                        return null;
                }
            }

            object ParseObject () {
                var objType = _type;
                object v = null;
                IDictionary dict = null;
                Type[] dictTypes = null;
                if (_type != null) {
                    v = Activator.CreateInstance (objType);
                    dict = v as IDictionary;
                    if (dict != null) {
                        dictTypes = objType.GetGenericArguments ();
                    }
                }
                // {
                jsonRead ();
                while (true) {
                    switch (PeekNextToken ()) {
                        case JsonToken.None:
                            return null;
                        case JsonToken.Comma:
                            continue;
                        case JsonToken.CurlyClose:
                            return v;
                        default:
                            // key :
                            var name = ParseString ();
                            if (name == null || PeekNextToken () != JsonToken.Colon) {
                                throw new Exception ("Invalid object format");
                            }
                            jsonRead ();

                            // value
                            if (objType != null) {
                                _type = dict != null ? dictTypes[1] : TypesCache.Instance.GetWantedType (objType, name);
                            }
                            var v1 = ParseValue ();
                            if (objType != null) {
                                if (v1 != null) {
                                    if (dict != null) {
                                        dict.Add (Convert.ChangeType (name, dictTypes[0], Extensions.NumberFormatInfo), v1);
                                    } else {
                                        TypesCache.Instance.SetValue (objType, name, v, v1);
                                    }
                                }
                            }
                            _type = objType;
                            break;
                    }
                }
            }

            object ParseArray () {
                bool isArray = false;
                Type arrType = _type;
                Type itemType = null;
                ArrayList list = null;
                if (_type != null) {
                    isArray = arrType.IsArray;
                    itemType = isArray ? arrType.GetElementType () : arrType.GetProperty ("Item").PropertyType;
                    _type = itemType;
                    list = GetArrayItem ();
                }
                // [
                jsonRead ();
                var parsing = true;
                while (parsing) {
                    switch (PeekNextToken ()) {
                        case JsonToken.None:
                            return null;
                        case JsonToken.Comma:
                            continue;
                        case JsonToken.SquaredClose:
                            parsing = false;
                            break;
                        default:
                            var v1 = ParseByToken (PeekNextToken ());
                            if (arrType != null) {
                                list.Add (Convert.ChangeType (v1, itemType, Extensions.NumberFormatInfo));
                                _type = itemType;
                            }
                            break;
                    }
                }
                object v = null;
                if (arrType != null) {
                    if (isArray) {
                        v = list.ToArray (itemType);
                        RecycleArrayItem (list);
                    } else {
                        v = Activator.CreateInstance (arrType);
                        var vList = v as IList;
                        if (vList == null) {
                            throw new Exception (string.Format ("Type '{0}' not compatible with array data", _type.Name));
                        }
                        foreach (var i in list) {
                            vList.Add (i);
                        }
                    }
                }
                return v;
            }

            string ParseString () {
                _stringBuf.Length = 0;
                char c;
                // "
                jsonRead ();
                bool parsing = true;
                while (parsing) {
                    if (jsonPeek () == -1) {
                        break;
                    }
                    c = GetNextChar ();
                    switch (c) {
                        case '"':
                            parsing = false;
                            break;
                        case '\\':
                            if (jsonPeek () == -1) {
                                throw new Exception ("Invalid string format");
                            }
                            c = GetNextChar ();
                            switch (c) {
                                case '"':
                                case '\\':
                                case '/':
                                    _stringBuf.Append (c);
                                    break;
                                case 'b':
                                    _stringBuf.Append ('\b');
                                    break;
                                case 'f':
                                    _stringBuf.Append ('\f');
                                    break;
                                case 'n':
                                    _stringBuf.Append ('\n');
                                    break;
                                case 'r':
                                    _stringBuf.Append ('\r');
                                    break;
                                case 't':
                                    _stringBuf.Append ('\t');
                                    break;
                                case 'u':
                                    for (int i = 0; i < 4; i++) {
                                        _hexBuf[i] = GetNextChar ();
                                    }
                                    _stringBuf.Append ((char) Convert.ToInt32 (new string (_hexBuf), 16));
                                    break;
                            }
                            break;
                        default:
                            _stringBuf.Append (c);
                            break;
                    }
                }
                return _stringBuf.ToString ();
            }

            object ParseNumber () {
                var numString = GetNextWord ();
                if (_type != null) {
                    var n = numString.ToFloatUnchecked ();
                    if (_type.IsEnum) {
                        return Enum.ToObject (_type, (int) n);
                    } else {
                        var nullableType = Nullable.GetUnderlyingType (_type);
                        return Convert.ChangeType (n, nullableType ?? _type, Extensions.NumberFormatInfo);
                    }
                }
                return null;
            }

            void SkipWhiteSpaces () {
                while (Char.IsWhiteSpace (PeekChar ())) {
                    jsonRead ();
                    if (jsonPeek () == -1) {
                        break;
                    }
                }
            }

            char PeekChar () {
                return Convert.ToChar (jsonPeek ());
            }

            char GetNextChar () {
                return jsonRead ();
            }

            string GetNextWord () {
                _stringBuf.Length = 0;
                while (!IsWordBreak (PeekChar ())) {
                    _stringBuf.Append (GetNextChar ());

                    if (jsonPeek () == -1) {
                        break;
                    }
                }
                return _stringBuf.ToString ();
            }

            JsonToken PeekNextToken () {
                SkipWhiteSpaces ();
                if (jsonPeek () == -1) {
                    return JsonToken.None;
                }
                switch (PeekChar ()) {
                    case '{':
                        return JsonToken.CurlyOpen;
                    case '}':
                        jsonRead ();
                        return JsonToken.CurlyClose;
                    case '[':
                        return JsonToken.SquaredOpen;
                    case ']':
                        jsonRead ();
                        return JsonToken.SquaredClose;
                    case ',':
                        jsonRead ();
                        return JsonToken.Comma;
                    case '"':
                        return JsonToken.String;
                    case ':':
                        return JsonToken.Colon;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return JsonToken.Number;
                }
                switch (GetNextWord ()) {
                    case "false":
                        return JsonToken.False;
                    case "true":
                        return JsonToken.True;
                    case "null":
                        return JsonToken.Null;
                }
                throw new Exception ("Invalid json");
            }

            public object ParseValue () {
                return ParseByToken (PeekNextToken ());
            }

            public void SetJson (string jsonString) {
                _json = jsonString;
                _jsonPos = 0;
            }

            public void SetType (Type type) {
                _type = type;
            }
        }

        sealed class TypesCache {
            public class TypeDesc {
                public readonly Dictionary<string, FieldInfo> Fields = new Dictionary<string, FieldInfo> (8);

                public readonly Dictionary<string, PropertyInfo> Properties = new Dictionary<string, PropertyInfo> (8);
            }

            public static readonly TypesCache Instance = new TypesCache ();

            readonly Dictionary<Type, TypeDesc> _types = new Dictionary<Type, TypeDesc> (32);

            readonly StringBuilder _sb = new StringBuilder (256);

            static readonly object _lock = new object ();

            public void SetValue (Type type, string name, object instance, object val) {
                var desc = GetCache (type);
                if (desc.Fields.ContainsKey (name)) {
                    desc.Fields[name].SetValue (instance, val);
                } else {
                    if (desc.Properties.ContainsKey (name)) {
                        desc.Properties[name].SetValue (instance, val, null);
                    }
                }
            }

            public Type GetWantedType (Type type, string name) {
                var desc = GetCache (type);
                if (desc.Fields.ContainsKey (name)) {
                    return desc.Fields[name].FieldType;
                }
                if (desc.Properties.ContainsKey (name)) {
                    return desc.Properties[name].PropertyType;
                }
                return null;
            }

            public void Clear () {
                lock (_lock) {
                    _types.Clear ();
                }
            }

            public TypeDesc GetCache (Type type) {
                TypeDesc desc;
                lock (_lock) {
                    if (!_types.ContainsKey (type)) {
                        desc = new TypeDesc ();
                        var ignoreType = typeof (JsonIgnoreAttribute);
                        var nameType = typeof (JsonNameAttribute);
                        string name;
                        foreach (var f in type.GetFields ()) {
                            if (f.IsPublic && !f.IsStatic && !f.IsInitOnly && !f.IsLiteral && !Attribute.IsDefined (f, ignoreType)) {
                                if (Attribute.IsDefined (f, nameType)) {
                                    name = ((JsonNameAttribute) Attribute.GetCustomAttribute (f, nameType)).Name;
                                    if (string.IsNullOrEmpty (name)) {
                                        name = f.Name;
                                    }
                                } else {
                                    name = f.Name;
                                }
                                _sb.Length = 0;
                                _sb.Append (name);
                                desc.Fields.Add (_sb.ToString (), f);
                            }
                        }
                        foreach (var p in type.GetProperties ()) {
                            if (p.CanRead && p.CanWrite && !Attribute.IsDefined (p, ignoreType)) {
                                if (Attribute.IsDefined (p, nameType)) {
                                    name = ((JsonNameAttribute) Attribute.GetCustomAttribute (p, nameType)).Name;
                                    if (string.IsNullOrEmpty (name)) {
                                        name = p.Name;
                                    }
                                } else {
                                    name = p.Name;
                                }
                                _sb.Length = 0;
                                _sb.Append (name);
                                desc.Properties.Add (_sb.ToString (), p);
                            }
                        }
                        _types[type] = desc;
                    } else {
                        desc = _types[type];
                    }
                }
                return desc;
            }
        }
    }
}