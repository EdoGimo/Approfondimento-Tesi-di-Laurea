using Geolocalizzazione.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Geolocalizzazione.Controllers
{
    public class HomeController : Controller
    {
        //connessione PostgreSQL con npgsql
        string connString = "Host=host;Username=usr;Password=psw;Database=db";

        //oppure ottenendo la stringa di connessione dal file Web.config
        //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public ActionResult Index()
        {
            ViewBag.mittente = "";
            ViewBag.tutti = "n";
            ViewBag.entro = "n";

            List<Indirizzo> model = new List<Indirizzo>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT i.id, i.via, i.numcivico, i.cap, i.citta, i.provincia, ST_asText(geom) as point, b.mittente " +
                    "from indirizzo i join busta b on b.destinatario = i.id " +
                    "order by b.mittente, i.id", con );
                NpgsqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        model.Add(
                           new Indirizzo()
                           {
                               id = reader.GetString(0),
                               via = reader.GetString(1),
                               numcivico = reader.GetString(2),
                               cap = reader.GetString(3),
                               citta = reader.GetString(4),
                               provincia = reader.GetString(5),
                               point = reader["point"].ToString(),
                               mittente = reader.GetString(7)
                           });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught: {0}", e);
                }
            }

            return View(model);
        }

        public ActionResult SelectAll(string mittente)
        {
            ViewBag.mittente = mittente;
            ViewBag.tutti = "y";
            ViewBag.entro = "n";

            List<Indirizzo> model = new List<Indirizzo>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT i.id, i.via, i.numcivico, i.cap, i.citta, i.provincia, ST_asText(i.geom) as point, b.mittente, " +
                        "ST_asText(i2.geom) as point_mittente," +
                        "CONCAT(i2.via, ' ', i2.numcivico, ' // ', i2.cap, ' ', i2.citta, ' ', i2.provincia) as info_mittente " +
                    "from indirizzo i join busta b on b.destinatario = i.id " +
                    "join indirizzo i2 on b.mittente = i2.id " +
                    "where b.mittente = @p", con);
                cmd.Parameters.AddWithValue("p", mittente);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        model.Add(
                           new Indirizzo()
                           {
                               id = reader.GetString(0),
                               via = reader.GetString(1),
                               numcivico = reader.GetString(2),
                               cap = reader.GetString(3),
                               citta = reader.GetString(4),
                               provincia = reader.GetString(5),
                               point = reader["point"].ToString(),
                               mittente = reader.GetString(7),
                               point_mittente = reader["point_mittente"].ToString(),
                               info_mittente = reader["info_mittente"].ToString()
                           });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught: {0}", e);
                }
            }
            return View("Index", model);
        }

        public ActionResult SelectIn(string mittente)
        {
            ViewBag.mittente = mittente;
            ViewBag.tutti = "y";
            ViewBag.entro = "y";

            List<Indirizzo> model = new List<Indirizzo>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT i.id, i.via, i.numcivico, i.cap, i.citta, i.provincia, ST_asText(i.geom) as point, " +
                        "b.mittente, ST_asText(i2.geom) as point_mittente, " +
                        "CONCAT(i2.via, ' ', i2.numcivico, ' // ', i2.cap, ' ', i2.citta, ' ', i2.provincia) as info_mittente " +
                    "from indirizzo i join busta b on b.destinatario = i.id " +
                    "join indirizzo i2 on b.mittente = i2.id " +
                    "where b.mittente = @p AND " +
                        "ST_DistanceSphere(i.geom, i2.geom) <= (5 * 1000) ", con);

                cmd.Parameters.AddWithValue("p", mittente);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        model.Add(
                           new Indirizzo()
                           {
                               id = reader.GetString(0),
                               via = reader.GetString(1),
                               numcivico = reader.GetString(2),
                               cap = reader.GetString(3),
                               citta = reader.GetString(4),
                               provincia = reader.GetString(5),
                               point = reader["point"].ToString(),
                               mittente = reader.GetString(7),
                               point_mittente = reader["point_mittente"].ToString(),
                               info_mittente = reader["info_mittente"].ToString()
                           });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught: {0}", e);
                }
            }
            return View("Index", model);
        }
    }
}