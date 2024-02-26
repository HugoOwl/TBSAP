using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula06_Exercicio_.Models
{
    public class LoginModel
    {
        public virtual String CompanyDB { get; set; }
        public virtual String Password { get; set; }
        public virtual String UserName { get; set; }
    }
}
