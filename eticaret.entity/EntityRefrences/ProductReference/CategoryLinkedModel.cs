namespace eticaret.entity.EntityRefrences.ProductReference
{
    public class CategoryLinkedModel
    {
        public int Level { get; set; }
        public string CategoryId { get; set; }
        public string NextCategory { get; set; }
        public string PrevCategory { get; set; }
    }
}