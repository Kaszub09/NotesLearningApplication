using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices.Entities {
    public class DatabaseClaim {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }


    }
}
