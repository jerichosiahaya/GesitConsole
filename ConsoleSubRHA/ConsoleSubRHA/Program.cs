using GesitConsole.Data;
using GesitConsole.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GesitConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateStatusJatuhTempo();
        }

        public static void UpdateStatusJatuhTempo()
        {
            var db = new GesitDbContext();
            try
            {
                var subRhaGetAll = db.SubRhas.OrderByDescending(s => s.CreatedAt).ToList();
                var dateNow = DateTime.Today.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));
                int compareDate = 0;
                foreach (var o in subRhaGetAll)
                {
                    var rhaGetById = db.Rhas.Where(p => p.Id == o.RhaId).FirstOrDefault();
                    var jatuhTempoRha = rhaGetById.StatusJt;
                    var jatuhTempoSubRha = o.JatuhTempo.ToString();
                    var jatuhTempoMerged = jatuhTempoSubRha + " " + jatuhTempoRha;
                    DateTime d1 = DateTime.ParseExact(dateNow, "d MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));
                    DateTime d2 = DateTime.ParseExact(jatuhTempoMerged, "d MMMM yyyy", new System.Globalization.CultureInfo("id-ID"));
                    compareDate = DateTime.Compare(d1, d2);
                    if (compareDate < 0)
                    {
                        o.StatusJatuhTempo = "Belum Jatuh Tempo";
                    }
                    else
                    {
                        o.StatusJatuhTempo = "Sudah Jatuh Tempo";
                    }
                }
                db.SaveChanges();
                Console.WriteLine("Data updated");
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(dbEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
