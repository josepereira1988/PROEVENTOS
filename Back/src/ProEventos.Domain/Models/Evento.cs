using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Identity;

namespace ProEventos.Domain.Models
{
    //[Table("EventosDetalhes")]
    public class Evento
    {
        //[Key]
        public int Id { get; set; }
        public string local { get; set; }
        public DateTime? DataEvento { get; set; }
        
        // [NotMapped]
        // public int ContagemDias { get; set; }

        [Required]
        public string Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string ImagemURL {get;set;}
        public string Telefone { get; set; }
        public string Email {get; set;}
        public int UserId { get; set; }
        public User User {get;set;}
        public IEnumerable<Lote> Lotes { get; set; }
        public IEnumerable<RedeSocial> RedeSociais { get; set; }
        public IEnumerable<PalestranteEvento> PalestranteEventos { get; set; }
    }
}
