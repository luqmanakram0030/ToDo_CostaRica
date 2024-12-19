using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDoCR.SharedDomain.Models
{
    public class CalcPatronal
    {
        public decimal Salario { get; set; }
        public List<CP_Categoria> ListaCategorias { get; set; }
        public CP_Resultado Resultado { get; set; }
        public CalcPatronal()
        {
            SetListaCategorias();
        }

        private void SetListaCategorias()
        {
            ListaCategorias = new List<CP_Categoria>() {
                new CP_Categoria("Caja Costarricense de Seguro Social",

                new List<CP_Rubro>(){
                    new CP_Rubro("SEM",                                         9.25M,  5.50M,Salario),
                    new CP_Rubro("IVM",                                         5.25M,  4.00M,Salario)
                }),

                new CP_Categoria("Recaudación Otras Instituciones",

                new List<CP_Rubro>(){
                    new CP_Rubro("Cuota Patronal Banco Popular",                0.25M,  0M,Salario),
                    new CP_Rubro("Asignaciones Familiares",                     5.00M,  0M,Salario),
                    new CP_Rubro("IMAS",                                        0.50M,  0M,Salario),
                    new CP_Rubro("INA",                                         1.50M,  0M,Salario)
                }),

                new CP_Categoria("Ley de Protección al Trabajador (LPT)",

                new List<CP_Rubro>(){
                    new CP_Rubro("Aporte Patrono Banco Popular",                0.25M,  0M,Salario),
                    new CP_Rubro("Fondo de Capitalización Laboral",             1.50M,  0M,Salario),
                    new CP_Rubro("Fondo de Pensiones Complementarias",          2.00M,  0M,Salario),
                    new CP_Rubro("Aporte Trabajador Banco Popular",             0M,     1.00M,Salario),
                    new CP_Rubro("INS",                                         1.00M,  0M,Salario)
                })
            };
        }

        public class CP_Resultado
        {
            public decimal PorcentajeTotalTrabajador { get; set; }
            public decimal PorcentajeTotalPatrono { get; set; }
            public decimal PorcentajeTotal { get; set; }
            public decimal MontoTotalTrabajador { get; set; }
            public decimal MontoTotalPatrono { get; set; }
            public decimal MontoTotal { get; set; }
        }

        public void Calcular(decimal salario)
        {
            Salario = salario;

            if (Resultado == null)
                Resultado = new CP_Resultado();

            SetListaCategorias();

            //foreach (var cat in ListaCategorias)
            //{
            //    foreach (var item in cat)
            //    {
            //        if (item.Patrono > 0)
            //            item.MontoPatrono = (Salario / 100) * item.Patrono;
            //        if (item.Trabajador > 0)
            //            item.MontoTrabajador = (Salario / 100) * item.Trabajador;
            //    }

            //}

            Resultado.MontoTotalPatrono = ListaCategorias.Sum(p => p.Sum(a => a.MontoPatrono));
            Resultado.MontoTotalTrabajador = ListaCategorias.Sum(p => p.Sum(a => a.MontoTrabajador));
            Resultado.MontoTotal = Resultado.MontoTotalPatrono + Resultado.MontoTotalTrabajador;

            Resultado.PorcentajeTotalPatrono = ListaCategorias.Sum(p => p.Sum(a => a.Patrono));
            Resultado.PorcentajeTotalTrabajador = ListaCategorias.Sum(p => p.Sum(a => a.Trabajador));
            Resultado.PorcentajeTotal = Resultado.PorcentajeTotalPatrono + Resultado.PorcentajeTotalTrabajador;

        }

        public class CP_Categoria : List<CP_Rubro>
        {
            public string Nombre { get; private set; }
            public CP_Categoria(string nombre, List<CP_Rubro> rubros) : base(rubros)
            {
                Nombre = nombre;
            }



        }

        public class CP_Rubro
        {
            public CP_Rubro(string concepto, decimal patrono, decimal trabajador, decimal salario)
            {
                Concepto = concepto;
                Patrono = patrono;
                Trabajador = trabajador;
                if (Patrono > 0)
                    MontoPatrono = (salario / 100) * Patrono;
                if (Trabajador > 0)
                    MontoTrabajador = (salario / 100) * Trabajador;
            }

            public string Concepto { get; set; }
            public decimal Patrono { get; set; }
            public decimal Trabajador { get; set; }
            public decimal MontoPatrono { get; set; }
            public decimal MontoTrabajador { get; set; }
        }
    }
}
