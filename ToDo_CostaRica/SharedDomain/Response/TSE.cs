using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoCR.SharedDomain.Response
{
    public class DatosPersonaTSE
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string ConocidoComo { get; set; }
        public string FechaNacimiento { get; set; }
        public string LugarNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Genero { get; set; }
        public string NombrePadre { get; set; }
        public string IdentificacionPadre { get; set; }
        public string NombreMadre { get; set; }
        public string IdentificacionMadre { get; set; }
        public string Empadronado { get; set; }
        public string Fallecido { get; set; }
        public string FechaDefuncion { get; set; }
        public string Marginal { get; set; }
        public string Edad { get; set; }
        public List<DatosMatrimoniosTSE> Matrimonios { get; set; }
        public List<DatosPersonaTSE> Hijos { get; set; }
        public DetalleLugarVotacion LugarVotacion { get; set; }
        public DetalleDefuncion Defuncion { get; set; }

        public DatosPersonaTSE()
        {

        }
    }
    public class DetalleDefuncion
    {
        public string CitaDefuncion { get; set; }
        public string FechaDefuncion { get; set; }
        public string NombreCompleto { get; set; }
        public string ConocidoComo { get; set; }
        public string LugarSuceso { get; set; }
        public string Marginal { get; set; }
        public DetalleDefuncion()
        {

        }
    }
    public class DatosMatrimoniosTSE
    {
        public string Cita { get; set; }
        public string CedulaConyuge { get; set; }
        public string NombreConyuge { get; set; }
        public string ConocidoComoConyuge { get; set; }
        public string LugarSuceso { get; set; }
        public string Fecha { get; set; }
        public string Tipo { get; set; }
        public string Extranjero { get; set; }
        public string Fallecido { get; set; }
        public string Marginal { get; set; }

        public DatosMatrimoniosTSE()
        {

        }
    }
    public class DetalleLugarVotacion
    {
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string DistritoAdministrativo { get; set; }
        public string DistritoElectoral { get; set; }
        public string NumeroElector { get; set; }
        public string CentroVotacion { get; set; }
        public string NumeroJunta { get; set; }
        public string FechaVencimientoCedula { get; set; }
        public string InscritoCantonDesde { get; set; }
        public string InscritoDistritoDesde { get; set; }

        public DetalleLugarVotacion()
        {

        }
    }

}
