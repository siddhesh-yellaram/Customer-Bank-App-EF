using BankLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository
{
    public class BankDBRepository:IBankRepository
    {
        private SqlConnection _sqlConnection;

        public BankDBRepository()
        {
            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString);
        }

        public void DoTrasanction(string name, string password, double amount, char type)
        {
            _sqlConnection.Open();
            var transaction = _sqlConnection.BeginTransaction();
            SqlCommand updateCommand = null;
            SqlCommand insertCommand = null;
            try
            {
                if (type == 'D')
                {
                    updateCommand = new SqlCommand("UPDATE BANK_MASTER SET BALANCE = BALANCE + @amount WHERE NAME lIKE @name AND PASSWORD = HASHBYTES('MD5',@password)", _sqlConnection, transaction);
                }
                else if (type == 'W')
                {
                    updateCommand = new SqlCommand("UPDATE BANK_MASTER SET BALANCE = BALANCE - @amount WHERE NAME lIKE @name AND PASSWORD = HASHBYTES('MD5',@password)", _sqlConnection, transaction);
                }
                updateCommand.Parameters.Add(new SqlParameter("@amount", amount));
                updateCommand.Parameters.Add(new SqlParameter("@name", name));
                updateCommand.Parameters.Add(new SqlParameter("@password", password));
                updateCommand.ExecuteNonQuery();

                insertCommand = new SqlCommand("INSERT INTO BANK_TRANSACTION VALUES(@name,@amount,@type,GETDATE())", _sqlConnection, transaction);
                Console.WriteLine(updateCommand);

                insertCommand.Parameters.Add(new SqlParameter("@name", name));
                insertCommand.Parameters.Add(new SqlParameter("@amount", amount));
                insertCommand.Parameters.Add(new SqlParameter("@type", type));
                insertCommand.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
            finally
            {
                if (_sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    _sqlConnection.Close();
                }
            }
        }

        public List<BankTransaction> GetTransactions(string name)
        {
            _sqlConnection.Open();
            var sqlCommand = new SqlCommand("SELECT * FROM BANK_TRANSACTION WHERE NAME LIKE @name", _sqlConnection);
            sqlCommand.Parameters.Add(new SqlParameter("@name", name));

            List<BankTransaction> transaction = new List<BankTransaction>();
            var dr = sqlCommand.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Console.WriteLine("{0}\t{1}", dr[0], dr[1]);
                    string names = dr.GetString(0);

                    double amount = Convert.ToDouble(dr[1]);
                    char type = Convert.ToChar(dr[2]);
                    DateTime date = Convert.ToDateTime(dr[3]);

                    transaction.Add(new BankTransaction(names, amount, type, date));
                }
            }
            _sqlConnection.Close();
            return transaction;
        }

        public string Login(string name, string password)
        {
            _sqlConnection.Open();
            string username = "";

            var loginCommand = new SqlCommand("SELECT NAME FROM BANK_MASTER WHERE NAME = @name and PASSWORD = HASHBYTES('MD5',@password)", _sqlConnection);
            loginCommand.Parameters.Add(new SqlParameter("@name", name));
            loginCommand.Parameters.Add(new SqlParameter("@password", password));

            var dr = loginCommand.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    username = dr.GetString(0);
                }
            }

            _sqlConnection.Close();
            return username;
        }

        public void Register(string name, double balance, string password)
        {
            _sqlConnection.Open();
            var insertCommand = new SqlCommand("INSERT INTO BANK_MASTER VALUES(@name,@balance,HASHBYTES('MD5',@password))", _sqlConnection);

            insertCommand.Parameters.Add(new SqlParameter("@name", name));
            insertCommand.Parameters.Add(new SqlParameter("@password", password));
            insertCommand.Parameters.Add(new SqlParameter("@balance", balance));

            insertCommand.ExecuteNonQuery();

            _sqlConnection.Close();
        }
    }
}
