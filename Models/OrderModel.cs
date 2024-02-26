using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula06_Exercicio_.Models
{
    public class OrderModel
    {
        public  String DocEntry { get; set; }
        public  String DocNum { get; set; }
        public  String CardCode { get; set; }
        public  String Comments { get; set; }
        public  DateTime DocDate { get; set; }
        public  DateTime DocDueDate { get; set; }
        public  DateTime TaxDate { get; set; }

        public List<ItemModel> DocumentLines = new List<ItemModel>();
    }
}
