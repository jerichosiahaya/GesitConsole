using ConsoleNotification.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static GesitAPI.Dtos.ProgoDocumentDto;
using static GesitAPI.Dtos.ProgoProjectDto;
// Author: Jericho Cristofel Siahaya
// Created: Nov 3 2021
namespace ConsoleNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Log(w);
            }
            using (StreamReader r = File.OpenText("log.txt"))
            {
                DumpLog(r);
            }
            Console.WriteLine("Wait until program successfully executed...");
            UpdateStatusNotification();
            Console.WriteLine($"Program took: {stopwatch.Elapsed}");
            stopwatch.Stop();
        }

        public static void Log(TextWriter w)
        {
            w.WriteLine($"\nProgram executed on: {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        public static string UpdateStatusNotification()
        {
            var _db = new GesitDbContext();
            var config = ConfigurationManager.GetSection("serverSettings");
            var requestUrl = ConfigurationManager.AppSettings["ProgoUrl"];
            var apiKey = ConfigurationManager.AppSettings["ProgoKey"];
            var progoProjectAPI = ConfigurationManager.AppSettings["ProgoProjectAPI"];
            var progoDocumentAPI = ConfigurationManager.AppSettings["ProgoDocumentAPI"];

            var client = new RestClient(requestUrl);
            client.UseNewtonsoftJson();
            var request = new RestRequest(progoProjectAPI);
            request.AddHeader("progo-key", apiKey);
            var response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<RootProgoProject>(response.Content);

            if (result.data.Count > 0)
            { 
            try 
            { 
                var dateTimeNow = DateTime.Now;

                foreach (var o in result.data)
                {
                    ProgoProject progoProject = new ProgoProject();

                    // check the aip id
                    var countCheckAIP = _db.ProgoProjects.Where(p => p.AipId == o.AIPId).Count();

                    // insert if empty
                    if (countCheckAIP == 0)
                    {
                        progoProject.AipId = o.AIPId;
                        progoProject.NamaAip = o.NamaAIP;
                        progoProject.ProjectId = o.ProjectId;
                        progoProject.NamaProject = o.NamaProject;
                        progoProject.ProjectBudget = Convert.ToInt64(o.ProjectBudget);
                        progoProject.ProjectDemandValue = Convert.ToInt64(o.ProjectDemandValue);
                        progoProject.StrategicImportance = o.StrategicImportance;
                        progoProject.Durasi = Convert.ToInt32(o.Durasi);
                        progoProject.EksImplementasi = o.EksImplementasi;
                        progoProject.Divisi = o.Divisi;
                        progoProject.Lob = o.LOB;
                        progoProject.NamaLob = o.NamaLOB;
                        progoProject.Squad = o.Squad;
                        progoProject.NamaSquad = o.NamaSquad;
                        progoProject.TahunCreate = Convert.ToInt32(o.TahunCreate);
                        progoProject.PeriodeAip = Convert.ToInt32(o.PeriodeAIP);
                        progoProject.AplikasiTerdampak = o.AplikasiTerdampak;
                        progoProject.ProjectCategory = o.ProjectCategory;
                        progoProject.JenisPengembangan = o.JenisPengembangan;
                        progoProject.Pengembang = o.Pengembang;
                        progoProject.PpjtiPihakTerkait = o.PPJTIPihakTerkait;
                        progoProject.LokasiDc = o.LokasiDC;
                        progoProject.LokasiDrc = o.LokasiDRC;
                        progoProject.EstimasiBiayaCapex = Convert.ToInt64(o.EstimasiBiayaCapex);
                        progoProject.EstimasiBiayaOpex = Convert.ToInt64(o.EstimasiBiayaOpex);
                        progoProject.StatusAip = o.statusAIP;
                        _db.ProgoProjects.Add(progoProject);
                    }
                    // update if exists
                    else if (countCheckAIP == 1)
                    {
                        var dataProject = _db.ProgoProjects.Where(p => p.AipId == o.AIPId).FirstOrDefault();
                        dataProject.NamaAip = o.NamaAIP;
                        dataProject.ProjectId = o.ProjectId;
                        dataProject.NamaProject = o.NamaProject;
                        dataProject.ProjectBudget = Convert.ToInt64(o.ProjectBudget);
                        dataProject.ProjectDemandValue = Convert.ToInt64(o.ProjectDemandValue);
                        dataProject.StrategicImportance = o.StrategicImportance;
                        dataProject.Durasi = Convert.ToInt32(o.Durasi);
                        dataProject.EksImplementasi = o.EksImplementasi;
                        dataProject.Divisi = o.Divisi;
                        dataProject.Lob = o.LOB;
                        dataProject.NamaLob = o.NamaLOB;
                        dataProject.Squad = o.Squad;
                        dataProject.NamaSquad = o.NamaSquad;
                        dataProject.TahunCreate = Convert.ToInt32(o.TahunCreate);
                        dataProject.PeriodeAip = Convert.ToInt32(o.PeriodeAIP);
                        dataProject.AplikasiTerdampak = o.AplikasiTerdampak;
                        dataProject.ProjectCategory = o.ProjectCategory;
                        dataProject.JenisPengembangan = o.JenisPengembangan;
                        dataProject.Pengembang = o.Pengembang;
                        dataProject.PpjtiPihakTerkait = o.PPJTIPihakTerkait;
                        dataProject.LokasiDc = o.LokasiDC;
                        dataProject.LokasiDrc = o.LokasiDRC;
                        dataProject.EstimasiBiayaCapex = Convert.ToInt64(o.EstimasiBiayaCapex);
                        dataProject.EstimasiBiayaOpex = Convert.ToInt64(o.EstimasiBiayaOpex);
                        dataProject.StatusAip = o.statusAIP;
                        _db.SaveChanges();
                    }

                    // get progo documents
                    var requestDocuments = new RestRequest(progoDocumentAPI + o.AIPId + "-" + dateTimeNow.Year);
                    requestDocuments.AddHeader("progo-key", apiKey);
                    var responseDocuments = client.Execute(requestDocuments);
                    var resultDocuments = JsonConvert.DeserializeObject<RootProgoDocument>(responseDocuments.Content);

                    if (resultDocuments.data.Count > 0)
                    {
                        foreach (var i in resultDocuments.data)
                        {
                            ProgoDocument progoDocument = new ProgoDocument();

                            // check file name first
                            var countCheckFileName = _db.ProgoDocuments.Where(p => p.NamaFile.Equals(i.NamaFile)).Count();

                            if (countCheckFileName == 0)
                            {
                                progoDocument.AipId = o.AIPId;
                                progoDocument.JenisDokumen = i.JenisDokumen;
                                progoDocument.TaskId = i.TaskId;
                                progoDocument.Tahun = dateTimeNow.Year;
                                progoDocument.NamaFile = i.NamaFile;
                                progoDocument.UrlDownloadFile = i.UrlDownloadFile;
                                _db.ProgoDocuments.Add(progoDocument);
                            }
                        }
                    }
                }
                _db.SaveChanges();

                // comparing to notification table
                var dataNotification = _db.Notifications.Where(o => o.Status == 0).ToList();
                foreach (var p in dataNotification)
                {
                    if (p.ProjectDocument.Equals("Requirement"))
                    {
                        var checkReq = _db.ProgoDocuments.Where(o => o.JenisDokumen.Equals("Memo Requirement") && o.AipId == p.ProjectId).Count();
                        if (checkReq >= 1)
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Cost & Benefit Analysis"))
                    {
                        var checkCost = _db.ProgoDocuments.Where(o => o.JenisDokumen.Equals("Cost & Benefit  Analysis") && o.AipId == p.ProjectId).Count();
                        if (checkCost >= 1)
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Severity Sistem"))
                    {
                        var checkSev = _db.ProgoDocuments.Where(o => o.JenisDokumen.Equals("Severity Sistem") && o.AipId == p.ProjectId).Count();
                        if (checkSev >= 1)
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Bussiness Impact Analysis"))
                    {
                        var checkBus = _db.ProgoDocuments.Where(o => o.JenisDokumen.Equals("Bussiness Impact Analysis") && o.AipId == p.ProjectId).Count();
                        if (checkBus >= 1)
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Izin / Lapor Regulator"))
                    {
                        var checkKajian = _db.ProgoDocuments.Where(o => o.JenisDokumen.Equals("Kajian untuk ijin/lapor regulatori") && o.AipId == p.ProjectId).Count();
                        if (checkKajian >= 1)
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Budgeting Capex / Opex"))
                    {
                        var checkAnggaran = _db.ProgoDocuments.Where(o => o.JenisDokumen.Equals("Anggaran atau Ijin Prinsip (Capex/Opex)") && o.AipId == p.ProjectId).Count();
                        if (checkAnggaran >= 1)
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Arsitektur / Topologi"))
                    {
                        var checkArsi = _db.ProgoDocuments.Where(o => o.JenisDokumen.Equals("Arsitektur atau topologi(AAD)") && o.AipId == p.ProjectId).Count();
                        if (checkArsi >= 1)
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Risk"))
                    {
                        var checkRisk = _db.ProgoDocuments.Where(o => o.JenisDokumen.Equals("Asement Risk ") && o.AipId == p.ProjectId).Count();
                        if (checkRisk >= 1)
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Target Implementasi"))
                    {
                        var checkTarget = _db.ProgoProjects.Where(o => o.AipId == p.ProjectId).Select(o => o.EksImplementasi).FirstOrDefault();
                        if (checkTarget == "")
                        {
                            p.Status = 0;
                        }
                        else
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Kategori Project"))
                    {
                        var checkProject = _db.ProgoProjects.Where(o => o.AipId == p.ProjectId).Select(o => o.ProjectCategory).FirstOrDefault();
                        if (checkProject == "")
                        {
                            p.Status = 0;
                        }
                        else
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Sistem / App Impact"))
                    {
                        var checkAppTerdampak = _db.ProgoProjects.Where(o => o.AipId == p.ProjectId).Select(o => o.AplikasiTerdampak).FirstOrDefault();
                        if (checkAppTerdampak == "")
                        {
                            p.Status = 0;
                        }
                        else
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("Pengadaan / In House"))
                    {
                        var checkPengadaan = _db.ProgoProjects.Where(o => o.AipId == p.ProjectId).Select(o => o.Pengembang).FirstOrDefault();
                        if (checkPengadaan == "")
                        {
                            p.Status = 0;
                        }
                        else
                        {
                            p.Status = 1;
                        }
                    }
                    else if (p.ProjectDocument.Equals("New / Enhance"))
                    {
                        var checkNewEnhance = _db.ProgoProjects.Where(o => o.AipId == p.ProjectId).Select(o => o.JenisPengembangan).FirstOrDefault();
                        if (checkNewEnhance == "")
                        {
                            p.Status = 0;
                        }
                        else
                        {
                            p.Status = 1;
                        }
                    }
                }
                _db.SaveChanges();
            }
                catch (DbUpdateException dbEx)
                {
                    return dbEx.Message;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "Program executed successfully";
        }

    }
}
