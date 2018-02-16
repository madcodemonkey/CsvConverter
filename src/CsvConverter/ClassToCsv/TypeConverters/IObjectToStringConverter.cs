using System;

namespace CsvConverter.ClassToCsv
{
    public interface IObjectToStringConverter
    {
        ConverterSettingForBooleansEnum BooleanSetting { get; set; }
        string Convert(Type theType, object value, string stringFormat);

        /// <summary>Adds a converter for a given type.</summary>
        /// <param name="theType">Type to convert</param>
        /// <param name="callback">Method to call when type is encountered</param>
        void AddConverter(Type theType, ObjectToStringDelegate callback);

        /// <summary>Indicates if a converter exists.  This is usually used prior to adding a converter to avoid an exception.</summary>
        /// <param name="theType">Type to convert</param>
        bool ConverterExists(Type theType);

        /// <summary>Removes a converter for a given type.</summary>
        /// <param name="theType">Type of converter to remove.</param>
        void RemoveConverter(Type theType);

    }
}
