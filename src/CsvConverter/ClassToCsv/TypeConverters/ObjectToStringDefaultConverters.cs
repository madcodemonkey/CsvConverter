using System;
using System.Collections.Generic;
using CsvConverter.Shared;

namespace CsvConverter.ClassToCsv
{
    public delegate string ObjectToStringDelegate(object rawData, string stringFormat, bool nullable);

    public class ObjectToStringDefaultConverters : IObjectToStringDefaultConverters
    {
        public ObjectToStringDefaultConverters()
        {
            RegisterBuiltInConverters();
        }

        public ConverterSettingForBooleansEnum BooleanSetting { get; set; } = ConverterSettingForBooleansEnum.UseTrueAndFalse;

        /// <summary>Converts a property into a string with an optional string format.</summary>
        /// <param name="theType">The type to convert.  Currently it only handles primitives.</param>
        /// <param name="obj">Object to convert (if using defaults this should be a property on the class)</param>
        /// <param name="stringFormat">Any special formatting</param>
        /// <returns></returns>
        public string Convert(Type theType, object value, string stringFormat)
        {
            if (value == null)
               return null;

            if (_converters.ContainsKey(theType))
            {
                return _converters[theType](value, stringFormat, theType.HelpIsNullable());
            }

            return value.ToString();
        }

        public void AddConverter(Type theType, ObjectToStringDelegate callback)
        {
            _converters.Add(theType, callback);
        }

        public void RemoveConverter(Type theType)
        {
            _converters.Remove(theType);
        }

        public bool ConverterExists(Type theType)
        {
            return _converters.ContainsKey(theType);
        }

        #region Private
        private Dictionary<Type, ObjectToStringDelegate> _converters = new Dictionary<Type, ObjectToStringDelegate>();

        private string ConvertBool(object rawData, string stringFormat, bool nullable)
        {
            bool data;
            if (nullable)
            {
                var rawNull = (bool?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (bool)rawData;
            }

            switch (BooleanSetting)
            {
                case ConverterSettingForBooleansEnum.UseTrueAndFalse:
                    return data ? "True" : "False";
                case ConverterSettingForBooleansEnum.UseOneAndZero:
                    return data ? "1" : "0";
                case ConverterSettingForBooleansEnum.UseTandF:
                    return data ? "T" : "F";
                case ConverterSettingForBooleansEnum.UseYandN:
                    return data ? "Y" : "N";
                case ConverterSettingForBooleansEnum.UseYesAndNo:
                    return data ? "Yes" : "No";
            }

            return data.ToString();
        }

        private string ConvertByte(object rawData, string stringFormat, bool nullable)
        {
            byte data;
            if (nullable)
            {
                var rawNull = (byte?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (byte)rawData;
            }

            return data.ToString(stringFormat);
        }

        private string ConvertDateTime(object rawData, string stringFormat, bool nullable)
        {
            DateTime data;
            if (nullable)
            {
                var rawNull = (DateTime?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (DateTime)rawData;
            }

            return data.ToString(stringFormat);
        }

        private string ConvertDecimal(object rawData, string stringFormat, bool nullable)
        {
            decimal data;
            if (nullable)
            {
                var rawNull = (decimal?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (decimal)rawData;
            }

            return data.ToString(stringFormat);
        }

        private string ConvertDouble(object rawData, string stringFormat, bool nullable)
        {
            double data;
            if (nullable)
            {
                var rawNull = (double?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (double)rawData;
            }

            return data.ToString(stringFormat);
        }

        private string ConvertFloat(object rawData, string stringFormat, bool nullable)
        {
            float data;
            if (nullable)
            {
                var rawNull = (float?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (float)rawData;
            }

            return data.ToString(stringFormat);
        }

        private string ConvertInt(object rawData, string stringFormat, bool nullable)
        {
            int data;
            if (nullable)
            {
                var rawNull = (int?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (int)rawData;
            }

            return data.ToString(stringFormat);
        }

        private string ConvertLong(object rawData, string stringFormat, bool nullable)
        {
            long data;
            if (nullable)
            {
                var rawNull = (long?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (long)rawData;
            }

            return data.ToString(stringFormat);
        }

        private string ConvertShort(object rawData, string stringFormat, bool nullable)
        {
            short data;
            if (nullable)
            {
                var rawNull = (short?)rawData;
                if (rawNull.HasValue == false)
                    return null;
                data = rawNull.Value;
            }
            else
            {
                data = (short)rawData;
            }

            return data.ToString(stringFormat);
        }

        private void RegisterBuiltInConverters()
        {
            AddConverter(typeof(int), ConvertInt);
            AddConverter(typeof(int?), ConvertInt);
            AddConverter(typeof(decimal), ConvertDecimal);
            AddConverter(typeof(decimal?), ConvertDecimal);
            AddConverter(typeof(double), ConvertDouble);
            AddConverter(typeof(double?), ConvertDouble);
            AddConverter(typeof(byte), ConvertByte);
            AddConverter(typeof(byte?), ConvertByte);
            AddConverter(typeof(bool), ConvertBool);
            AddConverter(typeof(bool?), ConvertBool);
            AddConverter(typeof(short), ConvertShort);
            AddConverter(typeof(short?), ConvertShort);
            AddConverter(typeof(float), ConvertFloat);
            AddConverter(typeof(float?), ConvertFloat);
            AddConverter(typeof(long), ConvertLong);
            AddConverter(typeof(long?), ConvertLong);
            AddConverter(typeof(DateTime), ConvertDateTime);
            AddConverter(typeof(DateTime?), ConvertDateTime);
        }
        #endregion // Private
    }
}
