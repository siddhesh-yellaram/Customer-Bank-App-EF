using BankLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository
{
    public interface IBankRepository
    {
        List<BankTransaction> GetTransactions(string name);

        void DoTrasanction(string name, string password, double balance, char type);

        void Register(string name, double balance, string password);

        string Login(string name, string password);
    }
}
