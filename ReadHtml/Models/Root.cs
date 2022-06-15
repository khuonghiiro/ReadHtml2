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

        public override bool Equals(object? obj)
        {
            var result = false;

            var root = obj as Root;

            if (root != null)
            {
                result = this.Index == root.Index
                    && this.Type == root.Type
                    && this.Value == root.Value
                    && this.FileName == root.FileName
                    && this.Avatar == root.Avatar
                    && this.Size == root.Size
                    && this.Image == root.Image
                    && this.Quote == root.Quote
                    && this.ListValue == root.ListValue
                    && this.ListRowImage == root.ListRowImage
                    && this.StarNameCaption == root.StarNameCaption
                    && this.Caption == root.Caption;
                return result;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
