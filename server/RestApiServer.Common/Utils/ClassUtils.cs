using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog;

namespace RestApiServer.Common.Utils
{
    //This needs some work to make it null-safe, or even null-proof.  
    //If it was done by Chuck Norris, even null is a non-null value, because if it is null according to him, it does not exist, literally :D
    public static class ClassUtils
    {
        public static T CopyFromBaseclass<T, U>(T dst, U src)
            where T : class
            where U : class
        {
            // Return null if either object is null
            if (src == null || dst == null) return null;

            CopyProps(src, dst);
            return dst;
        }
        private static void CopyProps(object src, object dst)
        {
            var srcType = src.GetType();
            var dstType = dst.GetType();

            foreach (var srcProp in srcType.GetProperties())
            {
                if (srcProp.CanRead)
                {
                    var dstProp = dstType.GetProperty(srcProp.Name);
                    if (dstProp?.CanWrite == true && dstProp.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                    {
                        try
                        {
                            var value = srcProp.GetValue(src);
                            dstProp.SetValue(dst, value);
                        }
                        catch (TargetException ex)
                        {
                            Log.Error($"Error setting property {srcProp.Name}: {ex.Message}");
                        }
                    }
                }
            }
        }

        private static object DeepCopyInternal(object? src, Dictionary<object, object> visited)
        {
            if (src == null) return null; // Return null if the source is null.
            if (visited.TryGetValue(src, out var existing)) return existing;

            var type = src.GetType();

            if (type.IsPrimitive || type.IsEnum || type.IsValueType || type == typeof(string))
                return src;

            if (type.IsArray)
            {
                var array = (Array)src;
                var elementType = type.GetElementType();
                var copiedArray = Array.CreateInstance(elementType, array.Length);
                visited[src] = copiedArray;
                for (int i = 0; i < array.Length; i++)
                {
                    copiedArray.SetValue(DeepCopyInternal(array.GetValue(i), visited), i);
                }
                return copiedArray;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var copiedCollection = CreateNewInstance(type);
                visited[src] = copiedCollection;

                if (copiedCollection is IDictionary dict && src is IDictionary srcDict)
                {
                    foreach (DictionaryEntry entry in srcDict)
                    {
                        dict.Add(DeepCopyInternal(entry.Key, visited), DeepCopyInternal(entry.Value, visited));
                    }
                }
                else if (copiedCollection is IList list && src is IList srcList)
                {
                    foreach (var item in srcList)
                    {
                        list.Add(DeepCopyInternal(item, visited));
                    }
                }
                else
                {
                    var addMethod = type.GetMethod("Add");
                    if (addMethod != null)
                    {
                        foreach (var item in (IEnumerable)src)
                        {
                            addMethod.Invoke(copiedCollection, new[] { DeepCopyInternal(item, visited) });
                        }
                    }
                }
                return copiedCollection;
            }

            var dst = Activator.CreateInstance(type);
            visited[src] = dst;

            foreach (var srcProp in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (srcProp.CanRead && srcProp.CanWrite && srcProp.GetIndexParameters().Length == 0)
                {
                    var value = srcProp.GetValue(src);
                    var copiedValue = DeepCopyInternal(value, visited);
                    srcProp.SetValue(dst, copiedValue);
                }
            }

            return dst;
        }

        private static object CreateNewInstance(Type type)
        {
            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                if (genericTypeDef == typeof(List<>))
                {
                    return Activator.CreateInstance(typeof(List<>).MakeGenericType(type.GetGenericArguments()));
                }
                if (genericTypeDef == typeof(Dictionary<,>))
                {
                    return Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments()));
                }
            }
            return Activator.CreateInstance(type);
        }

        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public bool Equals(object x, object y) => ReferenceEquals(x, y);
            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }
    }
}