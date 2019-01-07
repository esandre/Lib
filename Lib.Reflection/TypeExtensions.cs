using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Lib.Reflection
{
    public static class TypeExtensions
    {
        [PublicAPI]
        public static bool IsCore(this Type type)
        {
            if (type.IsPrimitive) return true;
            if (type.IsEnum) return true;
            if (type == typeof(object)) return true;
            if (type == typeof(string)) return true;
            if (type == typeof(DateTime)) return true;
            if (type == typeof(Guid)) return true;
            return false;
        }

        private static bool IsEnumerable(this Type type)
        {
            if (type.IsArray) return true;
            if (typeof(IEnumerable).IsAssignableFrom(type)) return true;
            return false;
        }

        [PublicAPI]
        public static bool IsGenericEnumerable(this Type type)
        {
            return type.IsEnumerable() && type.GenericTypeArguments.Any();
        }

        [PublicAPI]
        public static bool IsNonGenericEnumerable(this Type type)
        {
            return type.IsEnumerable() && !type.GenericTypeArguments.Any();
        }

        [PublicAPI]
        public static bool IsGenericEnumerableOf(this Type type, Predicate<Type> predicate)
        {
            return type.IsGenericEnumerable()
                   && predicate(type.GetGenericArguments().Single());
        }

        [PublicAPI]
        public static ConstructorInfo GetSingleConstructor(this Type type)
        {
            return type.GetConstructors().Single();
        }

        [PublicAPI]
        public static ConstructorInfo GetParameterlessContructor(this Type type)
        {
            return type.GetConstructor(new Type[0]);
        }

        [PublicAPI]
        public static ILookup<PropertyInfo, TAttribute> GetPropertiesWithAttribute<TAttribute>(this Type type) 
            where TAttribute : Attribute
        {
            return type.GetProperties()
                .SelectMany(propertyInfo => propertyInfo.GetCustomAttributes<TAttribute>()
                    .Select(attribute => (attribute: attribute, propertyInfo: propertyInfo)))
                .ToLookup(tuple => tuple.propertyInfo, tuple => tuple.attribute);
        }

        [PublicAPI]
        public static MethodInfo GetGenericMethod(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetGenericMethods(name, bindingFlags).Single();
        }

        private static IEnumerable<MethodInfo> GetGenericMethods(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetMethods(bindingFlags).Where(info => info.ContainsGenericParameters && info.Name == name);
        }

        [PublicAPI]
        public static bool IsOfGenericType(this Type typeToCheck, Type genericType)
        {
            if (!genericType.IsGenericTypeDefinition)
                throw new ArgumentException("The definition needs to be a GenericTypeDefinition", nameof(genericType));
            
            if (typeToCheck == null) return false;
            if (typeToCheck == typeof(object)) return false;
            if (typeToCheck == genericType) return true;

            if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
                return true;

            if (genericType.IsInterface && typeToCheck.GetInterfaces().Any(i => i.IsOfGenericType(genericType)))
                return true;

            return typeToCheck.BaseType.IsOfGenericType(genericType);
        }
    }
}
