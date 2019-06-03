using System.Collections.Generic;
using SistemaRH.Objects;
using static SistemaRH.Enumerators.GlobalEnums;
using System.Threading.Tasks;
using System;

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
    }
}