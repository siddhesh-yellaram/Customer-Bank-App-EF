using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLib
{
    public class BankTransaction
    {
        private string _name;
        private double _amount;
        private char _transactionType;
        private DateTime _date;

        public BankTransaction(string name, double amount, char transactionType, DateTime date)
        {
            _name = name;
            _amount = amount;
            _transactionType = transactionType;
            _date = date;
        }

        public string Name
        {
            get 
            { 
                return _name; 
            }
            set 
            { 
                _name = value; 
            }
        }
        public double Amount
        {
            get 
            { 
                return _amount; 
            }
            set 
            { 
                _amount = value; 
            }
        }
        public char TransactionType
        {
            get 
            { 
                return _transactionType; 
            }
            set { 
                _transactionType = value; 
            }
        }
        public DateTime Date
        {
            get 
            { 
                return _date; 
            }
            set { 
                _date = value; 
            }
        }
    }
}
