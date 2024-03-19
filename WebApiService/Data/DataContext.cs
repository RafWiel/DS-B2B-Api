using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            //modelBuilder.Entity<ServiceRequestModel>()
            //    .Property(p => p.CreationDate)
            //    .HasConversion(date => date.ToString("dd/MM/yyyy"),
            //                   str => DateTime.Parse(str));

            modelBuilder.HasDbFunction(() => DateToString(default))
                .HasTranslation(args => new SqlFunctionExpression(
                    functionName: "CONVERT",
                    arguments: args.Prepend(new SqlFragmentExpression("varchar(100)")),
                    nullable: true,
                    argumentsPropagateNullability: new[] { false, true, false },
                    type: typeof(string),
                    typeMapping: null));


            //// Configure model
            //modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(DataContext.DateToString)))
            //    .HasTranslation(e =>
            //    {
            //        return new SqlFunctionExpression("CONVERT",
            //            typeof(string),
            //            new[]{
            //                new SqlFragmentExpression("VARCHAR(100)"),
            //                e.First(),
            //                new SqlFragmentExpression("101"), // Pick a style available on T-SQL. 
            //            });
            //    });
        }

        //public static ValueConverter<DateTime, string> DateToString = new ValueConverter<DateTime, string>(
        //    dateTime => string.FromDateTimeUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)),
        //    instant => instant.ToDateTimeUtc()
        //);
        
        to nie dziala
        public static string DateToString(DateTime date)
        {
            return date.ToString("dd/MM/yyyy"); 
        }
    }
}
