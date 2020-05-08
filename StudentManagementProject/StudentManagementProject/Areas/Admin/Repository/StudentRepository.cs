using Dapper;
using StudentManagementProject.Areas.Admin.Models.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementProject.Areas.Admin.Repository
{
    public class StudentRepository
    {

        private readonly string _connectionString;

        public StudentRepository()
        {
            _connectionString = Startup.ConnectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public async Task<List<StudentDetailsViewModel>> GetStudents(int skip ,int take,string orderBy,string searchBy)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = (await dbConnection.QueryAsync<StudentDetailsViewModel>("select * from dbo.getstudents(@skip,@count,@orderby,@searchby)", new {skip=skip,count=take,orderby=orderBy,searchby=searchBy})).AsList();
                return query;
            }
        }

        public async Task<int> GetStudentsTotal()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = (await dbConnection.QueryAsync<int>("select COUNT(s.Id) from Students s inner join AspNetUsers u on u.Id = s.ApplicationUserId")).FirstOrDefault();
                return query;
            }
        }
    }
}
