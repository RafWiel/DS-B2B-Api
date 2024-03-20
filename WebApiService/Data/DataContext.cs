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
                .HasTranslation(args => new SqlFunctionExpression(
                    functionName: "cast",
                    arguments: new[]
                    { 
                        new SqlFragmentExpression($"datepart(year, convert(varchar, {args.First()}, 103)) as varchar(32)"),
                        //args.First(),
                        //new SqlFragmentExpression("as varchar(32)"),
                    },
                    nullable: true,
                    argumentsPropagateNullability: new[] { false },
                    type: typeof(string),
                    typeMapping: null));




            select

    'ZLS/' +
    convert(varchar(6), ordinal) +
            '/' +
    right('00' + convert(varchar(4), datepart(month, CreationDate)), 2) +
    '/' +
    convert(varchar(4), datepart(yyyy, CreationDate))
from ServiceRequests



            //modelBuilder.HasDbFunction(() => GetServiceRequestName(default, default))
            //    .HasTranslation(arguments =>
            //    {
            //        return new SqlFragmentExpression("COUNT(*) OVER()");
            //    });


        }

        public static string DateToString(DateTime date) => throw new NotSupportedException();
        public static string GetServiceRequestName(DateTime date, int ordinal) => throw new NotSupportedException();

        //public static string GetServiceRequestName1(int ordinal, DateTime date)
        //{
        //    return $"ZLS/{ordinal}/{date.ToString("MM")}/{date.ToString("yy")}";
        //}


        
    }
}
