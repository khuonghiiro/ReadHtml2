namespace ReadHtml.Models
{
    public class Size
    {
        public int? Width { get; set; }

        public int? Height { get; set; }

        public override bool Equals(object? obj)
        {
            var result = false;

            var size = obj as Size;

            if (size != null)
            {
                result = this.Width == size.Width
                    && this.Height == size.Height;
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
