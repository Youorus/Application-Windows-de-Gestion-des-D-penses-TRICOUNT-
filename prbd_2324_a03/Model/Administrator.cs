using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.Model;
public class Administrator : Users
{
    protected Administrator() { }

    public Administrator(string mail, string fullname, string password)
        : base(mail, password, password) {
        Role = Role.Administrator;
    }
}
