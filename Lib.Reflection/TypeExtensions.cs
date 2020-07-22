using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Lib.Reflection
{
    /// <summary>
    /// <see cref="Type"/> extensions
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// A core type is either a primitive type, an Enum, an object, a string, a DateTime or a Guid
        /// </summary>
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

        /// <summary>
        /// Is an <see cref="IEnumerable{T}"/>, but not a plain <see cref="IEnumerable"/>
        /// </summary>
        [PublicAPI]
        public static bool IsGenericEnumerable(this Type type)
        {
            return type.IsEnumerable() && type.GenericTypeArguments.Any();
        }

        /// <summary>
        /// Returns an implicit operator from {TImplicit} type
        /// </summary>
        [PublicAPI]
        public static MethodInfo GetImplicitOperator<TImplicit>(this Type type)
        {
            return type.GetMethod("op_Implicit", new[] {typeof(TImplicit)});
        }

        /// <summary>
        /// Is a plain <see cref="IEnumerable"/>, but not a generic <see cref="IEnumerable{T}"/>
        /// </summary>
        [PublicAPI]
        public static bool IsNonGenericEnumerable(this Type type)
        {
            return type.IsEnumerable() && !type.GenericTypeArguments.Any();
        }

        /// <summary>
        /// Is <see cref="IEnumerable{T}"/> with T matching predicate
        /// </summary>
        [PublicAPI]
        public static bool IsGenericEnumerableOf(this Type type, Predicate<Type> predicate)
        {
            return type.IsGenericEnumerable()
                   && predicate(type.GetGenericArguments().Single());
        }

        /// <summary>
        /// Returns the only constructor of a type, if exactly one
        /// </summary>
        [PublicAPI]
        public static ConstructorInfo GetSingleConstructor(this Type type)
        {
            return type.GetConstructors().Single();
        }

        /// <summary>
        /// Returns the parameterless constructor of a type, if any
        /// </summary>
        [PublicAPI]
        public static ConstructorInfo GetParameterlessContructor(this Type type)
        {
            return type.GetConstructor(new Type[0]);
        }

        /// <summary>
        /// Returns all properties of a type bearing a specific attribute
        /// </summary>
        /// <typeparam name="TAttribute">Searched attribute type</typeparam>
        /// <returns>Lookup mapping each found property with all attributes it bears</returns>
        [PublicAPI]
        public static ILookup<PropertyInfo, TAttribute> GetPropertiesWithAttribute<TAttribute>(this Type type) 
            where TAttribute : Attribute
        {
            return type.GetProperties()
                .SelectMany(propertyInfo => propertyInfo.GetCustomAttributes<TAttribute>()
                    .Select(attribute => (attribute, propertyInfo)))
                .ToLookup(tuple => tuple.propertyInfo, tuple => tuple.attribute);
        }

        /// <summary>
        /// Returns a single generic method with specific name and bindingFlags
        /// </summary>
        [PublicAPI]
        public static MethodInfo GetGenericMethod(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetGenericMethods(name, bindingFlags).Single();
        }

        private static IEnumerable<MethodInfo> GetGenericMethods(this Type type, string name, BindingFlags bindingFlags)
        {
            return type.GetMethods(bindingFlags).Where(info => info.ContainsGenericParameters && info.Name == name);
        }

        /// <summary>
        /// Checks if a type is the reification of a specific generic type
        /// </summary>
        /// <example>IEnumerable{string} is IEnumerable{}</example>
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

        /// <summary>
        /// Checks if a type is the reification of a specific generic type
        /// </summary>
        /// <example>IEnumerable{string} is IEnumerable{} but not ICollection{}</example>
        [PublicAPI]
        public static bool IsStrictlyOfGenericType(this Type typeToCheck, Type genericType)
        {
            if (!genericType.IsGenericTypeDefinition)
                throw new ArgumentException("The definition needs to be a GenericTypeDefinition", nameof(genericType));

            if (typeToCheck == null) return false;
            if (typeToCheck == typeof(object)) return false;
            if (typeToCheck == genericType) return true;

            if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
                return true;

            return typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType;
        }

        /// <summary>
        /// Find interfaces matching an unbound type in the haystack type.
        /// </summary>
        /// <example>With List{T} as haystack and IEnumerable{} as needle, will return IEnumerable{T}</example>
        [PublicAPI]
        public static IEnumerable<Type> FindInterfacesMatchingUnbound(this Type haystack, Type unboundNeedle)
        {
            return haystack.GetInterfaces().Where(t => t.IsStrictlyOfGenericType(unboundNeedle));
        }

        private static IEnumerable<Type> FlattenHierarchy(this Type type)
        {
            while (type != null && type != typeof(object))
            {
                yield return type;
                type = type.BaseType;
            }
        }

        [PublicAPI]
        public static Type As(this Type limitType, Type unboundNeedle)
        {
            if (limitType == unboundNeedle) return limitType;

            var correspondingParent = limitType.FlattenHierarchy()
                .FirstOrDefault(t => t.IsStrictlyOfGenericType(unboundNeedle));

            return correspondingParent != null 
                ? correspondingParent 
                : limitType.FindInterfacesMatchingUnbound(unboundNeedle).Single();
        }
    }
}
