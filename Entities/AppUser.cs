using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Extensions;

namespace Api.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;

        public string  Gender { get; set; }
        public string  Introduction { get; set; }
        
        public string  LookingForm { get; set; }
        public string  Interests { get; set; }
        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }
/* lo quitamos poruqe sino lo llama automapper directamente y llama a todos los campos
        public int GetAge(){
            return DateOfBirth.CalculateAge();
        }*/
    }

}