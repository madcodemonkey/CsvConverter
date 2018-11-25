using System;
using System.Reflection;

namespace CsvConverter
{
    /// <summary>Used for converting strings</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterStringAttribute : CsvConverterAttribute
    {
        /// <summary>Default constructor.  Do NOT use this unless you have overriden GetConverter!</summary>
        public CsvConverterStringAttribute() { }

        /// <summary>Use this if you have not override GetCoverter and want the default code to run and create a 
        /// type converters.</summary>
        /// <param name="converterType"></param>
        public CsvConverterStringAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

        /// <summary>Indicates that the converter is used PRIOR to type conversion.  In other words,
        /// you want to perform one or more pre-processes on the incoming string from a CSV file
        /// prior the the type converter getting a hold of it and converting it to the class
        /// property type.</summary>
        public bool IsPreConverter { get; set; } = false;

        /// <summary>Indicates that the converter is used AFTER type conversion.  In other words,
        /// the class property type should be converted to a string and then the post-processor
        /// should be called to perform additional operations on the string prior to be written
        /// out to the CSV file.</summary>
        public bool IsPostConverter { get; set; } = false;

        /// <summary>Indicates if the string operation is case sensitve.  The default is TRUE.
        /// Keep in mind that not all converters will use this setting.  For example, the
        /// CsvConverterStringTrimmer converter does NOT use it.</summary>
        public bool IsCaseSensitive { get; set; } = true;

        /// <summary>An optional, order for pre and post converters in case there are more than one decorating a property or class.</summary>
        public int Order { get; set; } = 999;

        public override ICsvConverter CreateConverterForProperty(Type theClassType, PropertyInfo propInfo, 
            IDefaultTypeConverterFactory defaultFactory)
        {
            bool isPreOrPostConverter = IsPreConverter || IsPostConverter;
            if (isPreOrPostConverter && IsColumIndexSpecified())
            {
                ThrowDoNotUseAttributePropertyException(theClassType, propInfo, "ColumnIndex");
            }

            if (isPreOrPostConverter && AreAltColumnNamesSpecified())
            {
                ThrowDoNotUseAttributePropertyException(theClassType, propInfo, "AltColumnNames");
            }

            if (isPreOrPostConverter && IsColumnNameSpecified())
            {
                ThrowDoNotUseAttributePropertyException(theClassType, propInfo, "ColumnName");
            }

            return base.CreateConverterForProperty(theClassType, propInfo, defaultFactory);
        }

        private void ThrowDoNotUseAttributePropertyException(Type theClassType, PropertyInfo propInfo, string nameOfAttributeProperty)
        {
            string typeOfConverter = IsPreConverter ? "PRE" : "POST";
            throw new CsvConverterAttributeException($"The '{propInfo.Name}' property on the {theClassType.Name} class " +
                $"specified a {nameOfAttributeProperty} in a {typeOfConverter} converter.  You should not use a {typeOfConverter} " +
                $"converter to specify {nameOfAttributeProperty} because it will NOT be used!");
        }
    }
}
