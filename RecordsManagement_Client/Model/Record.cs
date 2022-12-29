using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsManagement_Client.Model
{
    public class Record
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string performer = null!;
        public string Performer
        {
            get { return performer; }
            set { performer = value; }
        }

        private string title = null!;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private double price;
        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        private int stockCount = 0;
        public int StockCount
        {
            get { return stockCount; }
            set { stockCount = value; }
        }
    }
}
