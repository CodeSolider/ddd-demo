using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EbayPlatform.Infrastructure.Core.Extensions
{
    /// <summary>
    /// 程序集扩展类
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// 获取当前路径程序集
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetCurrentPathAssembly(this AppDomain domain)
        {
            var dlls = DependencyContext.Default.CompileLibraries
                .Where(x => !x.Name.StartsWith("Microsoft") && !x.Name.StartsWith("System"))
                .ToList();

            foreach (var dllItem in dlls)
            {
                if (dllItem.Type == "project")
                {
                    yield return Assembly.Load(dllItem.Name);
                }
            }
        }

        /// <summary>
        /// 判断指定的类型 是否是指定泛型类型的子类型，或实现了指定泛型接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }
            return false;

            // 测试某个类型是否是指定的原始接口
            bool IsTheRawGenericType(Type test)
                => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
        }
    }
}
