using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace FrameWork {
    public class Broadcast<T> {
        private readonly ObjectPool<Listener<T>> _pool = new ObjectPool<Listener<T>>(() => new Listener<T>());
        private readonly List<Listener<T>> _list = new List<Listener<T>>();
        
        public void On(Action<T> action, bool once = false) {
            var listener = this._pool.Get();
            listener.Init(action, once);
            this._list.Add(listener);
        }

        public void Once(Action<T> action) {
            this.On(action, true);
        }

        public void Emit(T param) {
            var needDestroy = false;
            foreach (var listener in this._list) {
                if (listener.destroyed) {
                    needDestroy = true;
                    continue;
                }
                listener.callback(param);
                if (listener.once) {
                    listener.destroyed = true;
                }
            }
            
            if(!needDestroy) return;
            
            for (var i = this._list.Count-1; i >=0; i--) {
                var listener = this._list[i];
                if(!listener.destroyed) continue;;
                this._list.RemoveAt(i);
                this._pool.Release(listener);
            }
        }

        public void Off(Action<T> action) {
            foreach (var listener in this._list) {
                if(!listener.callback.Equals(action)) continue;
                listener.destroyed = true;
            }
        }

        public void OffAll() {
            foreach (var listener in this._list) {
                this._pool.Release(listener);
                listener.destroyed = true;
            }
            this._list.Clear();
        }

        public void Destroy() {
            this._list.Clear();
            this._pool.Clear();
        }

        public bool Has(Action<T> action) {
            foreach (var listener in this._list) {
                if (listener.callback.Equals(action)) return true;
            }
            return false;
        }
    }

    public class Listener<T> {
        public Action<T> callback;
        public bool once;
        public bool destroyed;

        public void Init(Action<T> _callback, bool _once) {
            this.callback = _callback;
            this.once = _once;
            this.destroyed = false;
        }
    }
    
}