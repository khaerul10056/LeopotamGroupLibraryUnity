//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LeopotamGroup.Serialization.JsonInternal {
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
                            _sb.Append ('"');
                            _sb.Append (name);
                            _sb.Append ('"');
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
                            _sb.Append ('"');
                            _sb.Append (name);
                            _sb.Append ('"');
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