using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Drawing;
using System.Reflection.Metadata;
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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.HasDbFunction(() => GetServiceRequestType(default))
                .HasTranslation(args =>
                {
                    var typeColumn = args[0];
                    
                    return new SqlFunctionExpression(
                        functionName: "concat",
                        arguments: new[]
                        {
                            new SqlFragmentExpression($"CASE WHEN {typeColumn.Name}=1 THEN 'First' WHEN {typeColumn.Name}=2 THEN 'Second' WHEN {typeColumn.Name}=3 THEN 'Third' ELSE '' END"),
                            new SqlFragmentExpression("' do wywalenia, bez concat'"),
                        },
                        nullable: true,
                        argumentsPropagateNullability: new[] { false },
                        type: typeof(string),
                        typeMapping: null);
                });            

            //modelBuilder.HasDbFunction(() => GetServiceRequestName(default, default))
            //    .HasTranslation(arguments =>
            //    {
            //        return new SqlFragmentExpression("COUNT(*) OVER()");
            //    });
        }

        public static string DateToString(DateTime date) => throw new NotSupportedException();
        public static string GetServiceRequestName(int ordinal, DateTime date) => throw new NotSupportedException();
        public static string GetServiceRequestType(int type) => throw new NotSupportedException();

        //public static string GetServiceRequestName1(int ordinal, DateTime date)
        //{
        //    return $"ZLS/{ordinal}/{date.ToString("MM")}/{date.ToString("yy")}";
        //}



    }
}
