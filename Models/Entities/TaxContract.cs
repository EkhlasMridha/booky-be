using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Entities
{
    public class TaxContract
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double AdultsTax { get; set; }
        public double ChildrensTax { get; set; }
    }
}
