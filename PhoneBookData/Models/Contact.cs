using System;
using System.Collections.Generic;

namespace PhoneBookData.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }

        public int ContactTypeId { get; set; }
    }
}
