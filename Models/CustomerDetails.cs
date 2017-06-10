using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudWithRealmApp.Models
{
    public class CustomerDetails : RealmObject
    {
        [PrimaryKey]
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int CustomerAge { get; set; }
    }
}
