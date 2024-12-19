using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoCR.SharedDomain.Models.Loteria
{
    public class MiLoteriaGuardada
    {
        int id;
        string tipo;
        Guid token;
        public MiLoteriaGuardada()
        {
            token = Guid.NewGuid();
        }

        public DateTime Fecha { get; set; }

        public bool Ganador { get; set; }

        public string Icono
        {
            get
            {
                if (tipo == "loterianacional")
                    return "loteria_logo_loteria";
                else if (tipo == "lotto")
                    return "loteria_logo_lotto";
                else if (tipo == "chances")
                    return "loteria_logo_chances";
                else if (tipo == "nuevostiempos")
                    return "loteria_logo_nuevos_tiempos_reventados";
                else if (tipo == "Tres_Monazos")
                    return "loteria_logo_tresmonazos";
                return "";
            }
        }

        public int Id { get => id; set => id = value; }

        public int? Numero { get; set; }

        public int? Numero2 { get; set; }

        public int? Numero3 { get; set; }

        public int? Numero4 { get; set; }

        public int? Numero5 { get; set; }

        public bool Reventado { get; set; }

        public int? Serie { get; set; }

        public int? Sorteo { get; set; }

        public string Tanda { get; set; }
        public string Modalidad { get; set; }
        public DateTime? Notificado { get; set; }

        public string Tipo
        {
            get { return tipo; }
            set
            {

                tipo = value;
            }
        }

        public string TipoLiteral
        {
            get
            {
                if (tipo == "loterianacional")
                    return "Lotería Nacional";
                else if (tipo == "lotto")
                    return "Lotto";
                else if (tipo == "chances")
                    return "Chances";
                else if (tipo == "nuevostiempos")
                    return "Nuevos Tiempos";
                else if (tipo == "Tres_Monazos")
                    return "Tres Monazos";
                return "";
            }
        }

        public Guid Token { get => token; set => token = value; }

        public override string ToString()
        {
            if (Tipo == "loterianacional" || Tipo == "chances")
                return $"Número: {Numero} - Serie: {Serie}";
            else if (Tipo == "lotto")
                return $"{Numero} - {Numero2} - {Numero3} - {Numero4} - {Numero5}";
            else if (Tipo == "nuevostiempos")
                return $"Número: {Numero} - {(Reventado ? "R" : "-")}";
            else if (Tipo == "Tres_Monazos")
                return $"{Numero} - {Numero2} - {Numero3}";
            return "";
        }
    }
}
