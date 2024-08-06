using SV21T1020266.DomainModels;
using System;
using System.Data;
using Dapper;

namespace SV21T1020266.DataLayers.SQLServer
{
    public class CategoryDAL : _BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string conectionString) : base(conectionString)
        {

        }

        public int Add(Category data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO 
                            Categories (CategoryName, Description)
                            VALUES (@CategoryName, @Description);
                            SELECT @@IDENTITY";
                var parameters = new
                {
                    CategoryName = data.CategoryName ?? "",
                    Description = data.Description ?? ""
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT COUNT (*)
	                        from Categories 
	                        where CategoryName like @searchValue";
                var paramaters = new
                {
                    searchValue = $"%{searchValue}%"
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: paramaters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM Categories WHERE CategoryId = @CategoryId";
                var parameters = new
                {
                    CategoryId = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Category? Get(int id)
        {
            Category? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM Categories WHERE CategoryId = @CategoryId";
                var parameters = new { CategoryId = id };
                data = connection.QueryFirstOrDefault<Category>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> data = new List<Category>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * 
                            FROM (
	                            SELECT *,
		                            ROW_NUMBER() OVER (ORDER BY CategoryName) as RowNumber
	                            FROM Categories
	                            WHERE CategoryName LIKE @searchValue
                            ) AS t
                            WHERE @pageSize = 0
	                            OR (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            ORDER BY RowNumber;";
                var paramaters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = $"%{searchValue}%"
                };
                data = connection.Query<Category>(sql: sql, param: paramaters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Category data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Categories
                            SET CategoryName = @CategoryName,
                                Description = @Description
                            WHERE CategoryID = @CategoryID";
                var parameters = new
                {
                    CategoryID = data.CategoryID,
                    CategoryName = data.CategoryName,
                    Description = data.Description
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
