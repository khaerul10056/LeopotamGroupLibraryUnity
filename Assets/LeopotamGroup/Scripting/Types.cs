//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using LeopotamGroup.Common;

namespace LeopotamGroup.Scripting {
    enum ScriptVarType {
        Undefined = 0,
        String,
        Number
    }

    struct ScriptVar {
        public ScriptVarType Type { get; private set; }

        public ScriptVar (float data) : this () {
            AsNumber = data;
        }

        public ScriptVar (string data) : this () {
            AsString = data;
        }

        public string AsString {
            get { return IsUndefined ? "undefined" : (IsNumber ? _asNumber.ToNormalizedString () : _asString); }
            set {
                Type = ScriptVarType.String;
                _asString = value;
            }
        }

        public float AsNumber {
            get { return IsNumber ? _asNumber : 0f; }
            set {
                Type = ScriptVarType.Number;
                _asNumber = value;
            }
        }

        public bool IsUndefined {
            get { return Type == ScriptVarType.Undefined; }
        }

        public bool IsNumber {
            get { return Type == ScriptVarType.Number; }
        }

        public bool IsString {
            get { return Type == ScriptVarType.String; }
        }

        public void SetUndefined () {
            Type = ScriptVarType.Undefined;
        }

        string _asString;

        float _asNumber;
    }

    sealed class FunctionDesc {
        public int PC;

        public string[] Params;
    }

    sealed class Vars {
        readonly Dictionary<string, FunctionDesc> _functions = new Dictionary<string, FunctionDesc> (16);

        readonly Dictionary<string, ScriptVar> _vars = new Dictionary<string, ScriptVar> (128);

        readonly Dictionary<string, HostFunction> _hostFuncs = new Dictionary<string, HostFunction> (128);

        readonly ScriptVM _vm;

        public Vars (ScriptVM vm) {
            _vm = vm;
        }

        public void Reset () {
            _functions.Clear ();
            ResetVars ();
        }

        public void ResetVars () {
            _vars.Clear ();
        }

        public bool IsVarExists (string varName) {
            return _vars.ContainsKey (varName);
        }

        public void RegisterVar (string varName, ScriptVar v) {
            _vars[varName] = v;
        }

        public ScriptVar GetVar (string varName) {
            return _vars[varName];
        }

        public bool IsFunctionExists (string funcName) {
            return _functions.ContainsKey (funcName);
        }

        public void RegisterFunction (string funcName, int pc, List<string> paramList) {
            var desc = new FunctionDesc
            {
                PC = pc - 1,
            };
            if (paramList.Count > 0) {
                desc.Params = paramList.ToArray ();
            }
            _functions[funcName] = desc;
        }

        public FunctionDesc GetFunction (string varName) {
            return _functions[varName];
        }

        public bool IsHostFunctionExists (string funcName) {
            return _hostFuncs.ContainsKey (funcName);
        }

        public void RegisterHostFunction (string funcName, HostFunction cb) {
            _hostFuncs[funcName] = cb;
        }

        public void UnregisterHostFunctions () {
            _hostFuncs.Clear ();
        }

        public ScriptVar CallHostFunction (string funcName) {
            return _hostFuncs[funcName] (_vm);
        }
    }

    delegate ScriptVar HostFunction (ScriptVM vm);
}