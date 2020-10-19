using Domain.Model;
using Domain.Sql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Repositories.Sql
{
    public class DatabaseSimulatorContext : DbContext
    {
        public DatabaseSimulatorContext(DbContextOptions<DatabaseSimulatorContext> options) : base(options) { }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<LoginHistory> LoginHistorys { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<DataBase> DataBases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public static SqlResultModel TryAnswer(string connectingString, string sqlRequest)
        {
            var result = new SqlResultModel();
            try
            {
                SqlConnection sqlCon = new SqlConnection(connectingString);
                DataTable dtMain = new DataTable();
                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlRequest, sqlCon);
                sqlDa.Fill(dtMain);

                var rowsCount = dtMain.Rows.Count;
                var columnsCount = dtMain.Columns.Count;

                result.Columns = new List<string>();
                for (int j = 0; j < columnsCount; j++)
                {
                    result.Columns.Add(dtMain.Columns[j].ColumnName);
                }

                result.DataTable = new List<string[]>();
                for (int i = 0; i < rowsCount; i++)
                {
                    result.DataTable.Add(new string[columnsCount]);
                    for (int j = 0; j < columnsCount; j++)
                    {
                        result.DataTable[i][j] = dtMain.Rows[i][j].ToString();
                    }
                }
                result.HasException = false;
            }
            catch (Exception exp)
            {
                result.HasException = true;
                result.Exception = exp.Message;
            }

            return result;
        }
    }
}