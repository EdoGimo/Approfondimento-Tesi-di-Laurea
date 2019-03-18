namespace Geolocalizzazione.Models
{
    public class Indirizzo
    {
        public string id { get; set; }
        public string via { get; set; }
        public string numcivico { get; set; }
        public string cap { get; set; }
        public string citta { get; set; }
        public string provincia { get; set; }
        public string point { get; set; }
        public string mittente { get; set; }
        public string point_mittente { get; set; }
        public string info_mittente { get; set; }
    }
}