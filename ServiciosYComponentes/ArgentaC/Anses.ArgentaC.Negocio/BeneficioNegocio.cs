using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Dato;
using log4net;

namespace Anses.ArgentaC.Negocio
{
    public class BeneficioNegocio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BeneficioNegocio).Name);
        public static Persona BeneficioUVHI_Traer(Anses.ArgentaC.Negocio.UVHICreditos.CCInfo[] beneficios, Persona persona)
        {
            try
            {
                persona.BeneficiosRelacionados = new List<Beneficio>();
                bool tienenMismoCBU = true;
                if (beneficios != null)
                {
                    foreach (UVHICreditos.CCInfo c in beneficios)
                    {
                        Beneficio unBeneficio = new Beneficio();
                        unBeneficio.Cuil = long.Parse(c.RELCuil.ToString());
                        unBeneficio.NroBeneficio = c.NroBeneficio;
                        unBeneficio.ApellidoNombre = c.ApeNom;
                        unBeneficio.TipoDoc = Int16.Parse(c.DocTipo.ToString());
                        unBeneficio.NroDoc = long.Parse(c.DocNumero.ToString());
                        unBeneficio.FecNacimiento = c.Nacimiento;
                        unBeneficio.FecFallecimiento = c.Fallecimimento;
                        unBeneficio.SueldoBruto = c.ValorBruto;
                        unBeneficio.AfectacionDisponible = c.AfectacionDisponible;
                        unBeneficio.Sexo = c.Sexo;
                        unBeneficio.PeriodoAlta = c.PeriodoAlta;
                        unBeneficio.CBU = FormarCBU(c.CBU1, c.CBU2);
                        unBeneficio.CodBanco = c.Banco;
                        unBeneficio.CodAgencia = c.Agencia;
                        unBeneficio.RelCuil = long.Parse(c.Cuil.ToString());
                        unBeneficio.CodPrestacion = c.Prestacion;
                        unBeneficio.EsDiscapacitado = c.sEsDiscapacitado == "SI" ? true : false;
                        persona.BeneficiosRelacionados.Add(unBeneficio);
                    }

                    if (beneficios.Count() > 0)
                    {
                        string _cbu = FormarCBU(beneficios.First().CBU1, beneficios.First().CBU2);
                        short _banco = 0;
                        short _agencia = 0;
                        foreach (UVHICreditos.CCInfo c in beneficios)
                        {
                            if (FormarCBU(c.CBU1, c.CBU2) != _cbu)
                                tienenMismoCBU = false;
                            _banco = short.Parse(c.Banco.ToString());
                            _agencia = short.Parse(c.Agencia.ToString());
                        }
                        persona.CBU = tienenMismoCBU ? _cbu : null;
                        persona.codBanco = _banco;
                        persona.codAgencia = _agencia;
                    }
                }
                return persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona BeneficioSUAF_Traer(Anses.ArgentaC.Negocio.UVHICreditos.CCInfo[] beneficios, Persona persona)
        {
            try
            {
                persona.BeneficiosRelacionados = new List<Beneficio>();
                bool tienenMismoCBU = true;
                if (beneficios != null)
                {
                    foreach (UVHICreditos.CCInfo c in beneficios)
                    {
                        Beneficio unBeneficio = new Beneficio();
                        unBeneficio.Cuil = long.Parse(c.RELCuil.ToString());
                        unBeneficio.NroBeneficio = c.NroBeneficio;
                        unBeneficio.ApellidoNombre = c.ApeNom;
                        unBeneficio.TipoDoc = Int16.Parse(c.DocTipo.ToString());
                        unBeneficio.NroDoc = long.Parse(c.DocNumero.ToString());
                        unBeneficio.FecNacimiento = c.Nacimiento;
                        unBeneficio.FecFallecimiento = c.Fallecimimento;
                        unBeneficio.SueldoBruto = c.ValorBruto;
                        unBeneficio.AfectacionDisponible = c.AfectacionDisponible;
                        unBeneficio.Sexo = c.Sexo;
                        unBeneficio.PeriodoAlta = c.PeriodoAlta;
                        unBeneficio.CBU = FormarCBU(c.CBU1, c.CBU2);
                        unBeneficio.CodBanco = c.Banco;
                        unBeneficio.CodAgencia = c.Agencia;
                        unBeneficio.RelCuil = long.Parse(c.Cuil.ToString());
                        unBeneficio.CodPrestacion = c.Prestacion;
                        unBeneficio.EsDiscapacitado = c.sEsDiscapacitado == "SI" ? true : false;
                        persona.BeneficiosRelacionados.Add(unBeneficio);
                    }

                    if (beneficios.Count() > 0)
                    {
                        string _cbu = FormarCBU(beneficios.First().CBU1, beneficios.First().CBU2);
                        short _banco = 0;
                        short _agencia = 0;
                        foreach (UVHICreditos.CCInfo c in beneficios)
                        {
                            if (FormarCBU(c.CBU1, c.CBU2) != _cbu)
                                tienenMismoCBU = false;
                            _banco = short.Parse(c.Banco.ToString());
                            _agencia = short.Parse(c.Agencia.ToString());
                        }
                        persona.CBU = tienenMismoCBU ? _cbu : null;
                        persona.codBanco = _banco;
                        persona.codAgencia = _agencia;
                    }

                }
                return persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        public static Persona DeudasUVHI_Traer(Anses.ArgentaC.Negocio.UVHICreditos.Deuda[] deudas, Persona persona)
        {
            try
            {
                if (deudas == null)
                    persona.Deudas = null;
                else
                {
                    foreach (UVHICreditos.Deuda c in deudas)
                    {
                        Deuda unaDeuda = new Deuda();
                        unaDeuda.ImporteDeuda = c.Saldo;
                        unaDeuda.idSistema = c.SistemaCodigo;
                        unaDeuda.Sistema = c.Sistema;
                        persona.Deudas.Add(unaDeuda);
                    }
                }
                return persona;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return null;
            }
        }
        private static string FormarCBU(decimal cbu1, decimal cbu2)
        {
            string cbu_1 = cbu1.ToString();
            string cbu_2 = cbu2.ToString();

            while (cbu_1.Length < 8)
            {
                cbu_1 = string.Concat("0", cbu_1);
            }

            while (cbu_2.Length < 14)
            {
                cbu_2 = string.Concat("0", cbu_2);
            }

            return string.Concat(cbu_1, cbu_2);
        }
    }
}
