using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Khedmat_Shomar.DateBase
{
    public class MyContext :DbContext
    {
        public DbSet<Visit> Visits { get; set; }
    }
}