using System;

namespace CsvConverter.Core.IntegrationTests
{
    public class Graduation
    {
        [CsvConverterString(ColumnName = "First Name", ColumnIndex = 1)]
        public string FirstName { get; set; }

        [CsvConverterString(ColumnName = "Last Name", ColumnIndex = 2)]
        public string LastName { get; set; }

        // June 15, 1992 4:30 PM
        [CsvConverterDateTime(ColumnName = "Date and Time of Birth", ColumnIndex = 3, StringFormat = "MMMM d, yyyy h:mm tt")]
        public DateTime DateOfBirth { get; set; }

        // 8/12/2010  
        [CsvConverterDateTime(ColumnName = "High School Graduation Date", ColumnIndex = 4, StringFormat = "M/d/yyyy")]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch), OldValue = "N/A", NewValue = "", IsPreConverter = true)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch), OldValue = null, NewValue = "N/A", IsPostConverter = true)]
        public DateTime? HighSchoolGraduationDate { get; set; }

        [CsvConverterNumber(ColumnName = "High School Gradation Count", ColumnIndex = 5, StringFormat = "N0")]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch), OldValue = "N/A", NewValue = "", IsPreConverter = true)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch), OldValue = null, NewValue = "N/A", IsPostConverter = true)]
        public int? HighSchoolGradationCount { get; set; }

        // You could deleete NumberOfDecimalPlaces and specify AllowRounding = false and StringFormat = "P2" here as well and the output would be the same.
        [CsvConverterNumber(ColumnName = "High School Graduation Percentile", ConverterType = typeof(CsvConverterPercentage), ColumnIndex = 6, NumberOfDecimalPlaces = 4)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch), OldValue = "N/A", NewValue = "", IsPreConverter = true)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch), OldValue = null, NewValue = "N/A", IsPostConverter = true)]
        public decimal? HighSchoolGraduationPercentile { get; set; }

        [CsvConverterNumber(ColumnName = "High School GPA", ColumnIndex = 7, NumberOfDecimalPlaces = 2)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch), OldValue = "N/A", NewValue = "", IsPreConverter = true)]
        [CsvConverterStringOldAndNew(typeof(CsvConverterStringReplaceTextExactMatch), OldValue = null, NewValue = "N/A", IsPostConverter = true)]
        public decimal? HighSchoolGPA { get; set; }

        [CsvConverterBoolean(ColumnName = "Honor Student", ColumnIndex = 8, TrueValue = "True", FalseValue = "False")]
        public bool HonorStudent { get; set; }

        [CsvConverterString(ColumnName = "Mother's First Name", ColumnIndex = 9)]
        public string MotherFirstName { get; set; }

        [CsvConverterString(ColumnName = "Mother's Last Name", ColumnIndex = 10)]
        public string MotherLastName { get; set; }

        // 1972/5/27  
        [CsvConverterDateTime(ColumnName = "Mother's Birthdate", ColumnIndex = 11, StringFormat = "yyyy/M/d")]
        public DateTime? MotherBirthDate { get; set; }

        [CsvConverterString(ColumnIndex = 12)]
        public string Comments { get; set; }
    }
}
