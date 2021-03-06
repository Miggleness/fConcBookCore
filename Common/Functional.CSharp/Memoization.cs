﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Functional.CSharp
{
    public static partial class Memoization
    {
        // Listing 2.12 A simple example that clarifies how memoization works
        public static Func<T, R> Memoize<T, R>(Func<T, R> func) where T : IComparable //#A
        {
            Dictionary<T, R> cache = new Dictionary<T, R>();    //#B
            return arg =>                                       //#C
            {
                if (cache.ContainsKey(arg))                     //#D
                    return cache[arg];                          //#E
                return (cache[arg] = func(arg));                //#F
            };
        }

        // Listing 2.20 Thread-safe memoization function
        public static Func<T, R> MemoizeThreadSafe<T, R>(Func<T, R> func) where T : IComparable
        {
            ConcurrentDictionary<T, R> cache = new ConcurrentDictionary<T, R>();
            return arg => cache.GetOrAdd(arg, a => func(a));
        }

        // Listing 2.21 Thread-Safe Memoization function with safe lazy evaluation
        public static Func<T, R> MemoizeLazyThreadSafe<T, R>(Func<T, R> func) where T : IComparable
        {
            ConcurrentDictionary<T, Lazy<R>> cache = new ConcurrentDictionary<T, Lazy<R>>(); return arg => cache.GetOrAdd(arg, a => new Lazy<R>(() => func(a))).Value;
        }

        // Listing 2.5.1 Memoization with weak references
        public static Func<T, R> MemoizeWeakWithTtl<T, R>(Func<T, R> func, TimeSpan ttl)
            where T : class, IEquatable<T>
            where R : class
        {
            var keyStore = new ConcurrentDictionary<int, T>();

            T ReduceKey(T obj)
            {
                var oldObj = keyStore.GetOrAdd(obj.GetHashCode(), obj);
                return obj.Equals(oldObj) ? oldObj : obj;
            }

            var cache = new ConditionalWeakTable<T, Tuple<R, DateTime>>();

            Tuple<R, DateTime> FactoryFunc(T key) =>
                new Tuple<R, DateTime>(func(key), DateTime.Now + ttl);

            return arg =>
            {
                var key = ReduceKey(arg);
                var value = cache.GetValue(key, FactoryFunc);
                if (value.Item2 >= DateTime.Now)
                    return value.Item1;
                value = FactoryFunc(key);
                cache.Remove(key);
                cache.Add(key, value);
                return value.Item1;
            };
        }
    }

}
