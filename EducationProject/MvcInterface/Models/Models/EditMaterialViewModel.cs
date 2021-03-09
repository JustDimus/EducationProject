using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcInterface.Models.Models
{
    public class EditMaterialViewModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string URI { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Author { get; set; }

        public int Pages { get; set; }

        public int Duration { get; set; }

        public int Quality { get; set; }
    }
}
