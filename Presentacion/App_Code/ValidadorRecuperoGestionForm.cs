using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for ValidadorRecuperoGestionForm
/// </summary>
public class ValidadorRecuperoGestionForm
{
    #region valoresAValidar

    private string _valueMotivo;
    private string _valueEstado;
    private string _valorResidualDesde;
    private string _valorResidualHasta;

    #endregion

    public ValidadorRecuperoGestionForm(string valueMotivo, string valueEstado, string valorResidualDesde, string valorResidualHasta)
    {
        _valueMotivo = valueMotivo;
        _valueEstado = valueEstado;
        _valorResidualDesde = valorResidualDesde;
        _valorResidualHasta = valorResidualHasta;
    }

    public string EjecutarValidaciones()
    {
        var errorMessage = string.Empty;
        errorMessage += ValidarSeleccionDeDropdownList(_valueEstado, "Estado");
        errorMessage += ValidarSeleccionDeDropdownList(_valueMotivo, "Motivo");
        errorMessage += ValidarTextboxVacio(_valorResidualDesde, "Valor residual desde");
        errorMessage += ValidarTextboxVacio(_valorResidualHasta, "Valor residual hasta");
        errorMessage += ValidarInputSoloNumerico(_valorResidualDesde, "Valor residual desde");
        errorMessage += ValidarInputSoloNumerico(_valorResidualHasta, "Valor residual hasta");
        errorMessage += ValidaLongitudMaximaDeInput(_valorResidualHasta, "Valor residual hasta",12);
        errorMessage += ValidaLongitudMaximaDeInput(_valorResidualDesde, "Valor residual desde", 12);
        return errorMessage;

    }

    private string ValidarTextboxVacio(string textboxValue, string nombreDeTextbox)
    {
        if (string.IsNullOrEmpty(textboxValue))
        {
            return string.Format("Debe completar el campo {0} <br />", nombreDeTextbox);
        }
        return string.Empty;
    }

    private string ValidarSeleccionDeDropdownList(string selectedValue, string nombreDeDropdownList)
    {
        if (int.Parse(selectedValue) < 0)
        {
            return string.Format("Debe seleccionar una opcion del campo {0} <br />", nombreDeDropdownList);
        }
        return string.Empty;
    }

    private string ValidarInputSoloNumerico(string textBoxValue, string nombreDeTextbox)
    {
        Regex regex = new Regex(@"^[0-9]+([,.][0-9]+)?$");
        return regex.IsMatch(textBoxValue) ? string.Empty : string.Format("El campo {0} debe ser numérico <br/>", nombreDeTextbox);
    }

    private string ValidaLongitudMaximaDeInput(string textBoxValue, string nombreDeTextBox, int cantidadMaximaDeCaracteres)
    { 
        return textBoxValue.Length > cantidadMaximaDeCaracteres ? 
            string.Format("El campo {0} admite un máximo de {1} caracteres <br/>", nombreDeTextBox, cantidadMaximaDeCaracteres) 
            : string.Empty;
    }
}