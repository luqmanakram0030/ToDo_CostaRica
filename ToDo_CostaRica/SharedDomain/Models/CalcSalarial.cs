using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoCR.SharedDomain.Models
{
    public class CalcSalarial
    {
        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }
        public decimal SalarioQuincenal { get; private set; }
        public decimal CCSS { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal PorcentajeRetenido { get; set; }
        public decimal Renta { get; set; }
        public List<CS_RubroRenta> RentaRubros { get; set; }
        public CalcSalarial(decimal salario = 0)
        {
            if (salario > 0)
            {
                RentaRubros = new List<CS_RubroRenta>() {
                    new CS_RubroRenta(-1,863000,0),
                    new CS_RubroRenta(863001,1267000,10),
                    new CS_RubroRenta(1267001,2223000,15),
                    new CS_RubroRenta(2223001,4445000,20),
                    new CS_RubroRenta(4445001,-1,25),
            };
                SalarioBruto = salario;
                Calcular();
            }
        }

        public void Calcular()
        {
            CCSS = (SalarioBruto / 100M) * 10.5M;
            foreach (var item in RentaRubros)
            {
                if (SalarioBruto >= item.De)
                {
                    decimal hasta = item.Hasta != -1 ? item.Hasta : decimal.MaxValue;
                    decimal excedente = (SalarioBruto <= hasta ? SalarioBruto : hasta) - (item.De - 1);
                    item.Resultado = (excedente / 100M) * item.Porcentaje;
                }
            }
            Renta = RentaRubros.Sum(p => p.Resultado);
            TotalDeducciones = CCSS + Renta;
            SalarioNeto = SalarioBruto - TotalDeducciones;
            SalarioQuincenal = SalarioNeto / 2;
            PorcentajeRetenido = Math.Abs(((SalarioNeto / SalarioBruto) * 100) - 100);
        }

        public class CS_RubroRenta
        {
            public CS_RubroRenta(decimal de, decimal hasta, decimal porcentaje)
            {
                De = de;
                Hasta = hasta;
                Porcentaje = porcentaje;
                if (hasta == -1)
                {
                    Descripcion = $"Sobre el excedente de ₡{De:N0}";
                }
                else if (de == -1)
                {
                    Descripcion = $"Hasta ₡{Hasta:N0}";
                }
                else
                {
                    Descripcion = $"Sobre el excedente de ₡{De - 1:N0} hasta ₡{Hasta:N0}";
                }
            }

            public decimal De { get; }
            public decimal Hasta { get; }
            public decimal Porcentaje { get; }
            public string Descripcion { get; }
            public decimal Resultado { get; set; }
        }

    }


}
