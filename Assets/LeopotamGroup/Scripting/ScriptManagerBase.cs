//-------------------------------------------------------
// LeopotamGroupLibrary for unity3d
// Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>
//-------------------------------------------------------

using System.Collections.Generic;
using LeopotamGroup.Common;
using UnityEngine;

namespace LeopotamGroup.Scripting {
    abstract class ScriptManagerBase<T> : UnitySingleton<T> where T : MonoBehaviour {
        readonly ScriptVM _vm = new ScriptVM ();

        struct TimeoutPair {
            public float Time;

            public string Event;

            public ScriptVar? Param1;

            public ScriptVar? Param2;

            public ScriptVar? Param3;

            public ScriptVar? Param4;
        }

        readonly List<TimeoutPair> _timeoutListeners = new List<TimeoutPair> (4);

        protected override void OnConstruct () {
            base.OnConstruct ();
            OnAttachHostFunctions (_vm);
        }

        protected override void OnDestruct () {
            OnResetEvents ();
            base.OnDestruct ();
        }

        protected virtual void OnAttachHostFunctions (ScriptVM vm) {
            _vm.RegisterHostFunction ("callWithDelay", ApiCallWithDelay);
            _vm.RegisterHostFunction ("debug", ApiDebug);
        }

        protected virtual void OnRuntimeError (string errMsg) {
        }

        protected virtual void OnValidateEvents () {
            if (_timeoutListeners.Count > 0) {
                var time = Time.time;
                TimeoutPair pair;
                ScriptVar ret;
                string err;
                for (int i = _timeoutListeners.Count - 1; i >= 0; i--) {
                    if (_timeoutListeners[i].Time <= time) {
                        pair = _timeoutListeners[i];
                        err = _vm.CallFunction (pair.Event, out ret, pair.Param1, pair.Param2, pair.Param3, pair.Param4);
                        if (err != null) {
                            SetRuntimeError (err);
                            return;
                        }
                        _timeoutListeners.RemoveAt (i);
                    }
                }
            }
        }

        protected virtual void OnResetEvents () {
            _timeoutListeners.Clear ();
        }

        void LateUpdate () {
            OnValidateEvents ();
        }

        public void ResetEvents () {
            OnResetEvents ();
        }

        public void SetRuntimeError (string errMsg) {
            if (errMsg != null) {
                ResetEvents ();
                OnRuntimeError (errMsg);
            }
        }

        public string LoadSource (string sourceText) {
            return _vm.Load (sourceText);
        }

        public string CallFunction (string funcName, out ScriptVar result,
            ScriptVar? param1 = null, ScriptVar? param2 = null,
            ScriptVar? param3 = null, ScriptVar? param4 = null) {
            return _vm.CallFunction (funcName, out result, param1, param2, param3, param4);
        }

        public void CallFunctionWithDelay (string funcName, float timeout,
            ScriptVar? param1 = null, ScriptVar? param2 = null,
            ScriptVar? param3 = null, ScriptVar? param4 = null) {
            var pair = new TimeoutPair
            {
                Event = funcName,
                Time = Time.time + timeout,
                Param1 = param1,
                Param2 = param2,
                Param3 = param3,
                Param4 = param4
            };
            _timeoutListeners.Add (pair);
        }

        #region Common api

        ScriptVar ApiCallWithDelay (ScriptVM vm) {
            var count = vm.GetParamsCount ();
            var pTimeout = vm.GetParamByID (0);
            var pEvent = vm.GetParamByID (1);
            if (count < 2 || !pTimeout.IsNumber || !pEvent.IsString) {
                _vm.SetRuntimeError ("(nTimeout, sFuncName[, param1, param2]) parameters required");
                return new ScriptVar ();
            }
            ScriptVar? param1 = null;
            if (count > 2) {
                param1 = vm.GetParamByID (2);
            }
            ScriptVar? param2 = null;
            if (count > 3) {
                param2 = vm.GetParamByID (3);
            }
            ScriptVar? param3 = null;
            if (count > 4) {
                param3 = vm.GetParamByID (4);
            }
            ScriptVar? param4 = null;
            if (count > 5) {
                param4 = vm.GetParamByID (5);
            }

            CallFunctionWithDelay (pEvent.AsString, pTimeout.AsNumber, param1, param2, param3, param4);

            return new ScriptVar ();
        }


        ScriptVar ApiDebug (ScriptVM vm) {
            var str = string.Empty;
            for (int i = 0, iMax = vm.GetParamsCount (); i < iMax; i++) {
                str += " " + vm.GetParamByID (i).AsString;
            }
            Debug.LogFormat ("[SCRIPT: {0}]{1}", Time.time, str);
            return new ScriptVar ();
        }

        #endregion
    }
}