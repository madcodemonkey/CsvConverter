using CsvConverter.Reflection;
using System;
using System.Reflection;

namespace CsvConverter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class CsvConverterAttribute : Attribute
    {
        /// <summary>Default constructor.  Do NOT use this unless you have overriden GetConverter!</summary>
        public CsvConverterAttribute() { }

        /// <summary>Use this if you have not override GetCoverter and want the default code to run and create a 
        /// type converters.</summary>
        /// <param name="converterType"></param>
        public CsvConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

      

        /// <summary>READING CSV FILES: It is used ONLY when there is NOT a header row.  If the CSV file has a header row this is ignored.  
        /// If there is NOT a header row, this is mandatory and represents the column index (ONE based) position of the column in the CSV file.
        /// WRITING CSV FILES: It is used to determine the column order of the CSV files.  If not specified, we will order by ColumnName.</summary>
        public int ColumnIndex { get; set; } = int.MaxValue;

        /// <summary>READING CSV FILES:  Used ONLY when there IS a header row.  Use this if the CSV column name does NOT match the property name 
        /// (case doesn't matter when matching).  Only one column can be mapped to a property!  This is the primary column name and will
        /// be used BEFORE AltColumnNames!
        /// WRITING CSV FILES: If specified, this will be the header column name.  If not specified, the property name will be used.</summary>
        public string ColumnName { get; set; }

        /// <summary>READING CSV FILES: Used ONLY when there IS a header row and ColumnName was not used.  Use commas to specify more than one column name.  
        /// Only one column can be mapped to a property!
        /// WRITING CSV FILES:  Not used!</summary>
        public string AltColumnNames { get; set; }
    
        /// <summary>READING CSV FILES:  Ignore this property regardless if it is used in the CSV or if it has nothing to do with the CSV file.
        /// WRITING CSV FILES:  Not used!</summary>
        public bool IgnoreWhenReading { get; set; } = false;

        /// <summary>WRITING CSV FILES: Prevents data from being written to the CSV file. If ColumnIndex == -1 AND this is set to true, it is used to 
        /// prevent column from being created in the CSV file.
        /// READING CSV FILES:  Not used.</summary>
        public bool IgnoreWhenWriting { get; set; } = false;

        /// <summary>The class that will convert data.</summary>       
        public Type ConverterType { get; set; }

        /// <summary>When this attribute is used to decorate a class, you can target a particular target type. 
        /// It is NOT used when decorating a property! Also, when used on the class level it will NOT override
        /// converters on the actual property.</summary>
        public Type TargetPropertyType { get; set; }

        /// <summary>Creates a converter for a property on a class based on the type specified by ConverterType OR you can 
        /// dynamically create a converter based on the property information passed into the method.</summary>
        /// <param name="theClassType">The class were the property resides so that it can be named to help the user
        /// find the particular property in question if they have more than class decorated with converter attributes.</param>
        /// <param name="defaultFactory">The default type converter factory.</param>
        /// <returns>A converter</returns>
        public virtual ICsvConverter CreateConverterForClass(Type theClassType, IDefaultTypeConverterFactory defaultFactory)
        {
            // When decorating a class with this attribute, you MUST specify a target property type!
            if (TargetPropertyType == null)
            {
                throw new ArgumentException($"The {theClassType.Name} class specified an attribute that inherits from " +
                       $" {nameof(CsvConverterAttribute)}, but a {nameof(TargetPropertyType)} was NOT specified.");
            }

            if (IsColumIndexSpecified())
            {
                throw new CsvConverterAttributeException($"The {theClassType.Name} class has a class level attribute " +
                    $"that inherits {nameof(CsvConverterAttribute)} that is specifying a ColumIndex.  You can only " +
                    $"specify ColumnIndex if the attribute is on the property!");
            }

            if (AreAltColumnNamesSpecified())
            {
                throw new CsvConverterAttributeException($"The {theClassType.Name} class has a class level attribute " +
                    $"that inherits {nameof(CsvConverterAttribute)} that is specifying a AltColumnNames.  You can only " +
                    $"specify AltColumnNames if the attribute is on the property!");
            }

            if (IsColumnNameSpecified())
            {
                throw new CsvConverterAttributeException($"The {theClassType.Name} class has a class level attribute " +
                    $"that inherits {nameof(CsvConverterAttribute)} that is specifying a ColumnName.  You can only " +
                    $"specify ColumnName if the attribute is on the property!");
            }


            // If no converter type was specified by the user, use a default converter base on the target property type.
            if (ConverterType == null)
                ConverterType = defaultFactory.FindConverterType(this.TargetPropertyType);

            string errorMessage = GetErrorMessageForCreateConverterForClass(theClassType);

            var oneTypeConverter = ConverterType.HelpCreateAndCastToInterface<ICsvConverter>(errorMessage);

            oneTypeConverter.Initialize(this, defaultFactory);

            return oneTypeConverter;
        }


        /// <summary>Creates a converter for a property on a class based on the type specified by ConverterType OR ou can 
        /// dynamically create a converter based on the property information passed into the method.</summary>
        /// <param name="theClassType">The class were the property resides so that it can be named to help the user
        /// find the particular property in question if they have more than class decorated with converter attributes.</param>
        /// <param name="propInfo">Property information about the property that this attribute was on.</param>
        /// <param name="currentConversionDirection">Indicates what is being requested</param>
        /// <param name="defaultFactory">The default type converter factory.</param>
        /// <returns>A converter or null if one is not specified.</returns>
        public virtual ICsvConverter CreateConverterForProperty(Type theClassType, PropertyInfo propInfo, 
            IDefaultTypeConverterFactory defaultFactory)
        {
            // If no converter type was specified by the user, use a default converter based on the property's type.
            if (this.ConverterType == null)
                ConverterType = defaultFactory.FindConverterType(propInfo.PropertyType);

            string errorMessage = GetErrorMessageForCreateConverterForProperty(theClassType, propInfo);

            var oneTypeConverter = ConverterType.HelpCreateAndCastToInterface<ICsvConverter>(errorMessage);
                                                  
            oneTypeConverter.Initialize(this, defaultFactory);

            return oneTypeConverter;
        }


        /// <summary>Indicates if a column index was specified.</summary>
        public bool IsColumIndexSpecified()
        {
            return ColumnIndex != int.MaxValue;
        }

        /// <summary>Indicates if a column name was specified.</summary>
        public bool IsColumnNameSpecified()
        {
            return string.IsNullOrWhiteSpace(ColumnName) == false;
        }

        /// <summary>Indicates if alternate columns names were specified.</summary>
        public bool AreAltColumnNamesSpecified()
        {
            return string.IsNullOrWhiteSpace(AltColumnNames) == false;
        }

        /// <summary>When using the CreateConverterForProperty method, this will generate the long detailed error message
        /// to inform the user of a failure.</summary>
        protected string GetErrorMessageForCreateConverterForProperty(Type theClassType, PropertyInfo propInfo)
        {
            return $"The '{propInfo.Name}' property on the {theClassType.Name} class specified an attribute that inherits from " +
                $"{nameof(CsvConverterAttribute)} attribute, but there is a problem!  We could NOT create the converter it is specifying.  " +
                $"Please note, that all of converters should implement the {nameof(ICsvConverter)} intefaace so the {nameof(this.ConverterType)} " +
                $"type can be found.  The coverter specified in the attribute that inherits from " +
                $"{nameof(CsvConverterAttribute)} is not a proper converter.";
        }

        /// <summary>When using the CreateConverterForProperty method, this will generate the long detailed error message
        /// to inform the user of a failure.</summary>
        protected string GetErrorMessageForCreateConverterForClass(Type theClassType)
        {
            return $"The {theClassType.Name} class is using an attribute that inherits from the {nameof(CsvConverterAttribute)} " +
                $"attribute, but there is a problem!  We could NOT create the converter it is specifying.  Please note," +
                $"that all converters must implement the {nameof(ICsvConverter)} intefaace so the {nameof(this.ConverterType)} " +
                $"type can be found.  The converter found in the attribute that inherits from " +
                $"{nameof(CsvConverterAttribute)} is not a proper converter.";
        }

     
    }
}
