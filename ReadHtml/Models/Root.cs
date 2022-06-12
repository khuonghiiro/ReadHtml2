using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadHtml.Models
{
    public class Root
    {
        public string? Type { get; set; }

        public int? Index { get; set; }

        public string? Value { get; set; }

        public string? FileName { get; set; }

        public string? Avatar { get; set; }

        public Size? Size { get; set; }

        public Image? Image { get; set; }

        public string? Quote { get; set; }

        public List<Root>? ListValue { get; set; }

        public List<RowImage>? ListRowImage { get; set; }

        public string? StarNameCaption { get; set; }

        public string? Caption { get; set; }

    }
}
