using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RestApiServer.Utils
{
    public static class ClassUtils
    {
        public static T CopyFromBaseclass<T, U>(T dst, U src)
            where T : class
            where U : class
        {
            ArgumentNullException.ThrowIfNull(src);
            ArgumentNullException.ThrowIfNull(dst);

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
                    if (dstProp != null && dstProp.CanWrite && dstProp.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                    {
                        try
                        {
                            var value = srcProp.GetValue(src);
                            dstProp.SetValue(dst, value);
                        }
                        catch (Exception ex) when (ex is TargetException || ex is ArgumentException || ex is TargetInvocationException)
                        {
                            Console.WriteLine($"Error setting property {srcProp.Name}: {ex.Message}");
                        }
                    }
                }
            }
        }

        public static T DeepCopy<T>(T src) where T : class
        {
            if (src == null) throw new ArgumentNullException(nameof(src));

            var visited = new Dictionary<object, object>(new ReferenceEqualityComparer());

            return (T)DeepCopyInternal(src, visited);
        }

        private static object DeepCopyInternal(object src, Dictionary<object, object> visited)
        {
            if (visited.ContainsKey(src))
                return visited[src];

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
                else if (copiedCollection is ICollection collection)
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
            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return ReferenceEquals(x, y);
            }

            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            }
        }
    }
}