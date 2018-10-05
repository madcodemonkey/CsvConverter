using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CsvConverter.Reflection
{
    /// <summary>A place to hold non-creation reflection extensions.</summary>
    public static class ReflectionAttributeExtensions
    {
        /// <summary>Finds all the attributes of the specified type (T) on the class.</summary> 
        /// <typeparam name="T">The type to find</typeparam>
        /// <param name="theType">The type to search</param>
        /// <param name="inherit">Indicates if you want to search the base class of theType for attributes.</param>
        public static List<T> HelpFindAllClassAttributes<T>(this Type theType, bool inherit = true) where T : class
        {
            if (theType.IsClass == false)
                throw new ArgumentException("This method is intended for classes");

            var result = new List<T>();

            foreach (var oneAttributeAsObject in theType.GetCustomAttributes(typeof(T), inherit).ToList())
            {
                result.Add(oneAttributeAsObject as T);
            }

            return result;
        }

        /// <summary>Finds the first attribute of the specified type (T).</summary>
        /// <typeparam name="T">The type to find</typeparam>
        /// <param name="info">The Property Information which will be searched.</param>
        /// <param name="inherit">Indicates if you want to search the base class of theType for attributes.</param>
        public static T HelpFindAttribute<T>(this PropertyInfo info, bool inherit = true) where T : class
        {
            object oneAttributeAsObject = info.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
            if (oneAttributeAsObject == null)
                return null;
            return oneAttributeAsObject as T;
        }

        /// <summary>Finds all the attributes of the specified type (T).</summary>
        /// <typeparam name="T">The type to find</typeparam>
        /// <param name="info">The Property Information which will be searched.</param>
        /// <param name="inherit">Indicates if you want to search the base class of theType for attributes.</param>
        public static List<T> HelpFindAllAttributes<T>(this PropertyInfo info, bool inherit = true) where T : class
        {
            var result = new List<T>();

            foreach (var oneAttributeAsObject in info.GetCustomAttributes(typeof(T), inherit).ToList())
            {
                result.Add(oneAttributeAsObject as T);
            }

            return result;
        }
    }
}
