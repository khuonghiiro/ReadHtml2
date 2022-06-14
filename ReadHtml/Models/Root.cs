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

        public override bool Equals(Object obj)
        {
            if (obj is Root)
            {
                var that = obj as Root;
                return this.Index == that?.Index && 
                    this.Type == that?.Type && 
                    this.Value == that?.Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        //public override int GetHashCode()
        //{
        //    var hashcode = Index.GetHashCode();
        //    hashCode = Type.GetHashCode();
        //    hashCode = Value.GetHashCode();
        //    return hashCode;
        //}

    }
}
