namespace ScrapEra.CleanReader
{
    public class DomSerializationParams
    {
        public bool PrettyPrint { get; set; }

        public static DomSerializationParams CreateDefault()
        {
            return new DomSerializationParams();
        }
    }
}