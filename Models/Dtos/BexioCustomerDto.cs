using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Dtos
{
    public class BexioCustomerDto
    {
        private int? s_form=0;
        public int id { get; set; }
        public string nr { get; set; }
        public int contact_type_id { get; set; }
        public string name_1 { get; set; }
        public string name_2 { get; set; }
        public Nullable<int> salutation_id { get; set; }
        public int? salutation_form 
        {
            get
            {
                return s_form;
            }
            set
            {
                s_form=value ?? s_form;
            }
        }
        public Nullable<int> titel_id { get; set; }
        public string birthday { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public Nullable<int> country_id { get; set; }
        public string mail { get; set; }
        public string mail_second { get; set; }
        public string phone_fixed { get; set; }
        public string phone_fixed_second { get; set; }
        public string phone_mobile { get; set; }
        public string fax { get; set; }
        public string url { get; set; }
        public string skype_name { get; set; }
        public string remarks { get; set; }
        public Nullable<int> language_id { get; set; }
        public string contact_group_ids { get; set; }
        public string contact_branch_ids { get; set; }
        public int user_id { get; set; }
        public int owner_id { get; set; }
    }
}
