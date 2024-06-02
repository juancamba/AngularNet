using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public  bool IsMain { get; set; }
        public string PublicId { get; set; }

        // coin esto forzamos que las fotos pertenezca a un usuario y que cuando este se borre las fotos se borren
        public AppUser AppUser {get;set;}
        public int AppUserId { get; set; }
    }

}