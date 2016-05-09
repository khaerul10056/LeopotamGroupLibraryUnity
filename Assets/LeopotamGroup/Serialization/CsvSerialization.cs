//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LeopotamGroup.Serialization {
    /// <summary>
    /// Csv serialization. Supports deserialization only.
    /// </summary>
    public sealed class CsvSerialization {
        static readonly Regex _csvRegex = new Regex ("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");

        readonly List<string> _tokens = new List<string> (8);

        static readonly CsvSerialization _instance = new CsvSerialization ();

        void ParseLine (string data) {
            _tokens.Clear ();

            foreach (Match m in _csvRegex.Matches(data)) {
                var part = m.Value.Trim ();
                if (part.Length > 0) {
                    if (part[0] == '"' && part[part.Length - 1] == '"') {
                        part = part.Substring (1, part.Length - 2);
                    }
                    part = part.Replace ("\"\"", "\"");
                }
                _tokens.Add (part);
            }
        }

        /// <summary>
        /// Deserialize csv data from raw string source.
        /// </summary>
        /// <returns>Deserialized KeyValue-dictionary as Key from first column and lists of other columns as Value.</returns>
        /// <param name="data">Raw text data.</param>
        /// <param name="list">Target list if specified (useful for decrease GC allocations).</param>
        public Dictionary<string, string[]> Deserialize (string data, Dictionary<string, string[]> list = null) {
            if (list == null) {
                list = new Dictionary<string, string[]> ();
            }
            list.Clear ();

            var headerLen = -1;
            string key;
            using (var reader = new StringReader (data)) {
                while (reader.Peek () != -1) {
                    ParseLine (reader.ReadLine ());
                    if (headerLen == -1) {
                        headerLen = _tokens.Count;
                        if (headerLen < 2) {
                            #if UNITY_EDITOR
                            Debug.LogWarning ("Invalid csv header.");
                            #endif
                            break;
                        }
                    }
                    if (_tokens.Count != headerLen) {
                        #if UNITY_EDITOR
                        Debug.LogWarning ("Invalid csv line, skipping.");
                        #endif
                        continue;
                    }
                    key = _tokens[0];
                    _tokens.RemoveAt (0);
                    list.Add (key, _tokens.ToArray ());
                }
            }
            return list;
        }

        /// <summary>
        /// Deserialize csv data from raw string source with singleton csv deserializator.
        /// </summary>
        /// <returns>Deserialized KeyValue-dictionary as Key from first column and lists of other columns as Value.</returns>
        /// <param name="data">Raw text data.</param>
        /// <param name="list">Target list if specified (useful for decrease GC allocations).</param>
        public static Dictionary<string, string[]> DeserializeStatic (string data, Dictionary<string, string[]> list = null) {
            return _instance.Deserialize (data, list);
        }
    }
}