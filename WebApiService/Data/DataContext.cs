using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Drawing;
using System.Reflection.Metadata;
using WebApiService.Enums;
using WebApiService.Models;
using WebApiService.Services;

namespace WebApiService.Data
{
    public class DataContext : DbContext
    {
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<ServiceRequestModel> ServiceRequests { get; set; }

        //#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        //#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public static string DateToString(DateTime date) => throw new NotSupportedException();
        public static string GetCustomerType(byte type) => CustomerType.GetText(type);
        public static string GetCustomerTypeSorting(byte type) => throw new NotSupportedException();
        public static string GetEmployeeType(byte type) => throw new NotSupportedException();
        public static string GetEmployeeTypeSorting(byte type) => throw new NotSupportedException();
        public static string GetServiceRequestName(int ordinal, DateTime date) => throw new NotSupportedException();
        public static string GetServiceRequestNameSorting(int ordinal, DateTime date) => throw new NotSupportedException();
        public static string GetServiceRequestStatus(byte status) => throw new NotSupportedException();
        public static string GetServiceRequestStatusSorting(byte status) => throw new NotSupportedException();
        public static string GetServiceRequestSubmitType(byte type) => throw new NotSupportedException();
        public static string GetServiceRequestSubmitTypeSorting(byte type) => throw new NotSupportedException();
        public static string GetServiceRequestType(byte type) => throw new NotSupportedException();
        public static string GetServiceRequestTypeSorting(byte type) => throw new NotSupportedException();

        //public static string GetServiceRequestName(int ordinal, DateTime date)
        //{
        //    return $"ZLS/{ordinal}/{date.ToString("MM")}/{date.ToString("yy")}";
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Polish_CI_AS");

            #region Date

            modelBuilder.HasDbFunction(() => DateToString(default))
                .HasTranslation(args => new SqlFunctionExpression(
                    functionName: "convert",
                    arguments: new[]
                    {
                        new SqlFragmentExpression("varchar"),
                        args.First(),
                        new SqlFragmentExpression("103"),
                    },
                    nullable: true,
                    argumentsPropagateNullability: new[] { false },
                    type: typeof(string),
                    typeMapping: null));

            #endregion

            #region Customer type

            modelBuilder.HasDbFunction(() => GetCustomerType(default))
                .HasTranslation(args =>
                {
                    var typeColumn = (ColumnExpression)args[0];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{CustomerType.GetText(1)}' 
                                    when {typeColumn.Name} = 2 then '{CustomerType.GetText(2)}' 
                                    when {typeColumn.Name} = 3 then '{CustomerType.GetText(3)}' 
                                    else '' 
                                end as varchar(32)"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(string),
                        typeMapping: null);
                });

            modelBuilder.HasDbFunction(() => GetCustomerTypeSorting(default))
                .HasTranslation(args =>
                {
                    var typeColumn = (ColumnExpression)args[0];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{CustomerType.GetSorting(1)}' 
                                    when {typeColumn.Name} = 2 then '{CustomerType.GetSorting(2)}' 
                                    when {typeColumn.Name} = 3 then '{CustomerType.GetSorting(3)}' 
                                    else '' 
                                end as integer"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(int),
                        typeMapping: null);
                });

            #endregion

            #region Employee type

            modelBuilder.HasDbFunction(() => GetEmployeeType(default))
                .HasTranslation(args =>
                {
                    var typeColumn = (ColumnExpression)args[0];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{EmployeeType.GetText(1)}' 
                                    when {typeColumn.Name} = 2 then '{EmployeeType.GetText(2)}' 
                                    when {typeColumn.Name} = 3 then '{EmployeeType.GetText(3)}' 
                                    else '' 
                                end as varchar(32)"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(string),
                        typeMapping: null);
                });

            modelBuilder.HasDbFunction(() => GetEmployeeTypeSorting(default))
                .HasTranslation(args =>
                {
                    var typeColumn = (ColumnExpression)args[0];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{EmployeeType.GetSorting(1)}' 
                                    when {typeColumn.Name} = 2 then '{EmployeeType.GetSorting(2)}' 
                                    when {typeColumn.Name} = 3 then '{EmployeeType.GetSorting(3)}' 
                                    else '' 
                                end as integer"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(int),
                        typeMapping: null);
                });

            #endregion

            #region Service request name

            modelBuilder.HasDbFunction(() => GetServiceRequestName(default, default))
                .HasTranslation(args =>
                {
                    var ordinalColumn = (ColumnExpression)args[0];
                    var dateColumn = (ColumnExpression)args[1];

                    return new SqlFunctionExpression(
                        functionName: "concat",
                        arguments: new[]
                        {
                            new SqlFragmentExpression("'ZLS/'"),
                            new SqlFragmentExpression($"{ordinalColumn.Name}"),
                            new SqlFragmentExpression("'/'"),
                            new SqlFragmentExpression($"right('00' + convert(varchar(4), datepart(month, {dateColumn.Name})), 2)"),
                            new SqlFragmentExpression("'/'"),
                            new SqlFragmentExpression($"datepart(year, {dateColumn.Name})"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(string),
                        typeMapping: null);
                });

            modelBuilder.HasDbFunction(() => GetServiceRequestNameSorting(default, default))
                .HasTranslation(args =>
                {
                    var ordinalColumn = (ColumnExpression)args[0];
                    var dateColumn = (ColumnExpression)args[1];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                (
                                    {ordinalColumn.Name} + 
                                    datepart(month, {dateColumn.Name}) + 
                                    datepart(year, {dateColumn.Name})
                                ) as integer"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(int),
                        typeMapping: null);
                });

            #endregion

            #region Service request status

            modelBuilder.HasDbFunction(() => GetServiceRequestStatus(default))
               .HasTranslation(args =>
               {
                   var typeColumn = (ColumnExpression)args[0];

                   return new SqlFunctionExpression(
                       functionName: "cast",
                       arguments: new[]
                       {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{ServiceRequestStatus.GetText(1)}' 
                                    when {typeColumn.Name} = 2 then '{ServiceRequestStatus.GetText(2)}' 
                                    when {typeColumn.Name} = 3 then '{ServiceRequestStatus.GetText(3)}' 
                                    when {typeColumn.Name} = 4 then '{ServiceRequestStatus.GetText(4)}' 
                                    when {typeColumn.Name} = 5 then '{ServiceRequestStatus.GetText(5)}' 
                                    when {typeColumn.Name} = 6 then '{ServiceRequestStatus.GetText(6)}' 
                                    when {typeColumn.Name} = 7 then '{ServiceRequestStatus.GetText(7)}' 
                                    when {typeColumn.Name} = 8 then '{ServiceRequestStatus.GetText(8)}' 
                                    else '' 
                                end as varchar(32)"),
                       },
                       nullable: true,
                       argumentsPropagateNullability: new[] { false },
                       type: typeof(string),
                       typeMapping: null);
               });

            modelBuilder.HasDbFunction(() => GetServiceRequestStatusSorting(default))
               .HasTranslation(args =>
               {
                   var typeColumn = (ColumnExpression)args[0];

                   return new SqlFunctionExpression(
                       functionName: "cast",
                       arguments: new[]
                       {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{ServiceRequestStatus.GetSorting(1)}' 
                                    when {typeColumn.Name} = 2 then '{ServiceRequestStatus.GetSorting(2)}' 
                                    when {typeColumn.Name} = 3 then '{ServiceRequestStatus.GetSorting(3)}' 
                                    when {typeColumn.Name} = 4 then '{ServiceRequestStatus.GetSorting(4)}' 
                                    when {typeColumn.Name} = 5 then '{ServiceRequestStatus.GetSorting(5)}' 
                                    when {typeColumn.Name} = 6 then '{ServiceRequestStatus.GetSorting(6)}' 
                                    when {typeColumn.Name} = 7 then '{ServiceRequestStatus.GetSorting(7)}' 
                                    when {typeColumn.Name} = 8 then '{ServiceRequestStatus.GetSorting(8)}' 
                                    else '' 
                                end as integer"),
                       },
                       nullable: true,
                       argumentsPropagateNullability: new[] { false },
                       type: typeof(int),
                       typeMapping: null);
               });

            #endregion

            #region Service request submit type

            modelBuilder.HasDbFunction(() => GetServiceRequestSubmitType(default))
                .HasTranslation(args =>
                {
                    var typeColumn = (ColumnExpression)args[0];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{ServiceRequestSubmitType.GetText(1)}' 
                                    when {typeColumn.Name} = 2 then '{ServiceRequestSubmitType.GetText(2)}' 
                                    when {typeColumn.Name} = 3 then '{ServiceRequestSubmitType.GetText(3)}' 
                                    when {typeColumn.Name} = 4 then '{ServiceRequestSubmitType.GetText(4)}' 
                                    else '' 
                                end as varchar(32)"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(string),
                        typeMapping: null);
                });

            modelBuilder.HasDbFunction(() => GetServiceRequestSubmitTypeSorting(default))
                .HasTranslation(args =>
                {
                    var typeColumn = (ColumnExpression)args[0];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{ServiceRequestSubmitType.GetSorting(1)}' 
                                    when {typeColumn.Name} = 2 then '{ServiceRequestSubmitType.GetSorting(2)}' 
                                    when {typeColumn.Name} = 3 then '{ServiceRequestSubmitType.GetSorting(3)}' 
                                    when {typeColumn.Name} = 4 then '{ServiceRequestSubmitType.GetSorting(4)}' 
                                    else '' 
                                end as integer"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(int),
                        typeMapping: null);
                });

            #endregion

            #region Service request type

            modelBuilder.HasDbFunction(() => GetServiceRequestType(default))
                .HasTranslation(args =>
                {
                    var typeColumn = (ColumnExpression)args[0];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{ServiceRequestType.GetText(1)}' 
                                    when {typeColumn.Name} = 2 then '{ServiceRequestType.GetText(2)}' 
                                    when {typeColumn.Name} = 3 then '{ServiceRequestType.GetText(3)}' 
                                    else '' 
                                end as varchar(32)"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(string),
                        typeMapping: null);
                });

            modelBuilder.HasDbFunction(() => GetServiceRequestTypeSorting(default))
                .HasTranslation(args =>
                {
                    var typeColumn = (ColumnExpression)args[0];

                    return new SqlFunctionExpression(
                        functionName: "cast",
                        arguments: new[]
                        {
                            new SqlFragmentExpression(@$"
                                case
                                    when {typeColumn.Name} = 1 then '{ServiceRequestType.GetSorting(1)}' 
                                    when {typeColumn.Name} = 2 then '{ServiceRequestType.GetSorting(2)}' 
                                    when {typeColumn.Name} = 3 then '{ServiceRequestType.GetSorting(3)}' 
                                    else '' 
                                end as integer"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(int),
                        typeMapping: null);
                });

            #endregion

            //modelBuilder.HasDbFunction(() => GetServiceRequestName(default, default))
            //    .HasTranslation(arguments =>
            //    {
            //        return new SqlFragmentExpression("COUNT(*) OVER()");
            //    });
        }
    }
}
