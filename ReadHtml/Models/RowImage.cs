using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadHtml.Models
{
    public class RowImage
    {
        public int Row { get; set; } = 1;

        public List<Image>? listImage { get; set; }
    }
}
