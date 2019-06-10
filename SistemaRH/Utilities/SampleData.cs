using System.Collections.Generic;
using SistemaRH.Objects;
using static SistemaRH.Enumerators.GlobalEnums;
using System.Threading.Tasks;
using System;
using Android.App;
using Android.Content;

namespace SistemaRH.Utilities
{
    public class SampleData
    {
        private static SampleData sampleData;

        public static SampleData Instance
        {
            get
            {
                if (sampleData == null)
                    sampleData = new SampleData();
                return sampleData;
            }
        }

        public async Task GenerateAllSampleData()
        {
            if (Application.Context.GetSharedPreferences("app_data", FileCreationMode.Private).GetBoolean("first_time", true))
            {
                await CreateJobs();
                await CreateDepartments();
                await CreateCompetitions();
                await CreateTrainings();
            }    
        }

        public async Task CreateJobs()
        {
            List<Job> jobs = new List<Job>()
            {
                new Job()
                {
                    Name = "Desarrollador de software",
                    MinSalary = 20000,
                    MaxSalary = 50000,
                    RiskLevel = RiskLevel.Medium,
                    State = true
                },
                new Job()
                {
                    Name = "Desarrollador web",
                    MinSalary = 15000,
                    MaxSalary = 30000,
                    RiskLevel = RiskLevel.Low,
                    State = true
                },
                new Job()
                {
                    Name = "Soporte Tecnico",
                    MinSalary = 10000,
                    MaxSalary = 30000,
                    RiskLevel = RiskLevel.Medium,
                    State = true
                },
                new Job()
                {
                    Name = "Conserje",
                    MinSalary = 5000,
                    MaxSalary = 15000,
                    RiskLevel = RiskLevel.Low,
                    State = true
                },
                new Job()
                {
                    Name = "Analista de sistemas",
                    MinSalary = 20000,
                    MaxSalary = 50000,
                    RiskLevel = RiskLevel.High,
                    State = true
                },
                new Job()
                {
                    Name = "Diseñador grafico",
                    MinSalary = 10000,
                    MaxSalary = 30000,
                    RiskLevel = RiskLevel.Medium,
                    State = true
                },
                new Job()
                {
                    Name = "Diseñador de base de datos",
                    MinSalary = 30000,
                    MaxSalary = 60000,
                    RiskLevel = RiskLevel.High,
                    State = true
                },
                new Job()
                {
                    Name = "Secretaria",
                    MinSalary = 10000,
                    MaxSalary = 30000,
                    RiskLevel = RiskLevel.Low,
                    State = true
                },
                new Job()
                {
                    Name = "Contador",
                    MinSalary = 30000,
                    MaxSalary = 40000,
                    RiskLevel = RiskLevel.High,
                    State = false
                }
            };
            await MyLib.Instance.InsertObjectsAsync(jobs);
        }

        public async Task CreateDepartments()
        {
            List<Department> departments = new List<Department>()
            {
                new Department()
                {
                    Description = "Software",
                    State = true
                },
                new Department()
                {
                    Description = "Soporte tecnico",
                    State = true
                },
                new Department()
                {
                    Description = "Recursos Humanos",
                    State = true
                },
                new Department()
                {
                    Description = "Contabilidad",
                    State = false
                }
            };
            await MyLib.Instance.InsertObjectsAsync(departments);
        }

        public async Task CreateCompetitions()
        {
            List<Competition> competitions = new List<Competition>()
            {
                new Competition()
                {
                    Description = "Manejo de Recursos Humanos",                    
                    State = true
                },
                new Competition()
                {
                    Description = "Uso de Herramientas Ofimaticas",
                    State = true
                },
                new Competition()
                {
                    Description = "Gestion de Presupuesto",
                    State = true
                },
                new Competition()
                {
                    Description = "Hablar en publico",
                    State = false
                },
                new Competition()
                {
                    Description = "Buena comunicacion",
                    State = true
                },
                new Competition()
                {
                    Description = "Trabajo en equipo",
                    State = true
                }
            };
            await MyLib.Instance.InsertObjectsAsync(competitions);
        }

        public async Task CreateTrainings()
        {
            List<Training> trainings = new List<Training>()
            {
                new Training()
                {
                    Description = "Administracion de empresas",
                    FromDate = new DateTime(1,1,1),
                    ToDate = new DateTime(4,1,1),
                    TrainingLevel = TrainingLevel.Postgraduate,
                    Institution = "UNIBE"
                },
                new Training()
                {
                    Description = "Conocimientos de PHP",
                    FromDate = new DateTime(1,1,1),
                    ToDate = new DateTime(4,1,1),
                    TrainingLevel = TrainingLevel.Technical
                },
                new Training()
                {
                    Description = "Conocimientos de Java",
                    FromDate = new DateTime(1,1,1),
                    ToDate = new DateTime(6,1,1),
                    TrainingLevel = TrainingLevel.Grade
                },                
                new Training()
                {
                    Description = "Experiencia con HTML5 y CSS3",
                    FromDate = new DateTime(1,1,1),
                    ToDate = new DateTime(3,1,1),
                    TrainingLevel = TrainingLevel.Technical
                },
                new Training()
                {
                    Description = "Experiencia con bases de datos relacionales (Postgres, MySQL, etc)",
                    FromDate = new DateTime(1,1,1),
                    ToDate = new DateTime(4,1,1),
                    TrainingLevel = TrainingLevel.Grade
                }
            };
            await MyLib.Instance.InsertObjectsAsync(trainings);
        }
    }
}