namespace WebApplication3.Models
{
    public class Ganador
    {
        public int SubastaId { get; set; }
        public string SubastaTitulo { get; set; }
        public string UsuarioNombre { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime UltimaFechaPuja { get; set; }
    }
}
