//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d License
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System;
using LeopotamGroup.Scripting.Internal;

namespace LeopotamGroup.Scripting {
    sealed class ScriptVM {
        readonly Scanner _scanner;

        readonly Parser _parser;

        public ScriptVM () {
            _scanner = new Scanner ();
            _parser = new Parser (this, _scanner);
        }

        public string Load (string source) {
            if (string.IsNullOrEmpty (source)) {
                return "no source code";
            }
            var err = _scanner.Load (source);
            if (err != null) {
                return err;
            }
            err = _parser.Parse ();
            if (err != null) {
                return err;
            }
            return null;
        }

        public void SetRuntimeError (string msg) {
            _parser.SemErr (msg);
        }

        public string CallFunction (string funcName, out ScriptVar result,
            ScriptVar? param1 = null, ScriptVar? param2 = null,
            ScriptVar? param3 = null, ScriptVar? param4 = null) {
            var undef = new ScriptVar ();
            if (!_parser.Vars.IsFunctionExists (funcName)) {
                result = undef;
                return string.Format ("function '{0}' not found", funcName);
            }
            var func = _parser.Vars.GetFunction (funcName);
            _scanner.PC = func.PC;
            _parser.Vars.ResetVars ();
            var id = 0;
            var max = func.Params != null ? func.Params.Length : 0;
            if (param1 != null && id < max) {
                _parser.Vars.RegisterVar (func.Params[id++], param1.Value);
            }
            if (param2 != null && id < max) {
                _parser.Vars.RegisterVar (func.Params[id++], param2.Value);
            }
            if (param3 != null && id < max) {
                _parser.Vars.RegisterVar (func.Params[id++], param3.Value);
            }
            if (param4 != null && id < max) {
                _parser.Vars.RegisterVar (func.Params[id++], param4.Value);
            }
            for (; id < max; id++) {
                _parser.Vars.RegisterVar (func.Params[id], undef);
            }
            var err = _parser.CallFunction ();
            result = _parser.RetVal;
            return err;
        }

        public void RegisterHostFunction (string funcName, HostFunction cb) {
            _parser.Vars.RegisterHostFunction (funcName, cb);
        }

        public void UnregisterAllHostFunctions () {
            _parser.Vars.UnregisterHostFunctions ();
        }

        public int GetParamsCount () {
            return _parser.CallParams.Count - _parser.CallParamsOffset;
        }

        public ScriptVar GetParamByID (int id) {
            return id >= 0 && id < GetParamsCount () ? _parser.CallParams[_parser.CallParamsOffset + id] : new ScriptVar ();
        }

        public bool IsFunctionExists (string funcName) {
            return funcName != null && _parser.Vars.IsFunctionExists (funcName);
        }
    }
}