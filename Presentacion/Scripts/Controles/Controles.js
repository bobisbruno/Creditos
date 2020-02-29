
function Browser() {
    var ua, s, i;
    this.isIE = false;
    this.isNS = false;
    this.version = null;
    ua = navigator.userAgent;

    s = "MSIE";
    if ((i = ua.indexOf(s)) >= 0) {
        this.isIE = true;
        this.version = parseFloat(ua.substr(i + s.length));

        return;
    }

    s = "Netscape6/";
    if ((i = ua.indexOf(s)) >= 0) {
        this.isNS = true;
        this.version = parseFloat(ua.substr(i + s.length));
        return;
    }

    // Treat any other "Gecko" browser as NS 6.1.
    s = "Gecko";
    if ((i = ua.indexOf(s)) >= 0) {
        this.isNS = true;
        this.version = 6.1;
        return;
    }
}

var browser = new Browser();
var isNN = browser.isNS;
var isIE7 = browser.isIE;

function fechaCompleta(dia, mes, anio) {
    if (dia == '' || mes == '' || anio == '') {
        return false;
    }
    else {
        if (anio.length != 4) {
            return false;
        }
        else {
            return true;
        }
    }
}
function isDateError(obj) {
    try {
        var d = $('#<%= tblDate.ClientID %>').find(':input')[0].value;
        var m = $('#<%= tblDate.ClientID %>').find(':input')[1].value;
        var a = $('#<%= tblDate.ClientID %>').find(':input')[2].value;
        if (fechaCompleta(d, m, a)) {
            var retorno = true;
            var lblError = $('#<%= lbl_ErrorFecha.ClientID %>');
            if (!$('#<%= tblDate.ClientID %>').find(":input").attr("disabled") &&
                        !isDateValid(d, m, a)) {
                lblError.text('Debe ingresar una fecha válida.');
                lblError.style = "";
                retorno = false;
            }
            else {
                lblError.text('');
                lblError.style = "display:none";
                retorno = true;
            }
            return retorno;
        }
        else {
            return true;
        }
    }
    catch (e) {
        return false;
    }
} 

function isDateValid(dia, mes, anio) {
    if (parseInt(anio) > 1499 && parseInt(anio) < 2999) {
        if (parseInt(mes, 10) > 0 && parseInt(mes, 10) <= 12) {
            if (parseInt(dia, 10) > 0 && parseInt(dia, 10) <= daysInMonth(mes, anio))
                return true;
            else
                return false;
        }
        else
            return false;
    }
    else
        return false;
}

function daysInMonth(monthNum, yearNum) {
    var monthNum = Number(monthNum);
    var yearNum = Number(yearNum);    
    var d = new Date(yearNum, monthNum, 0);
    var day = d.getDate().toString();
    
    return day;
}

function cDate(sDate) {
    fecha = new Date(fecha);
    return fecha
}

/* Funciones para strings */
function trim(stringToTrim) {
    return stringToTrim.replace(/^\s+|\s+$/g, "");
}

function PadLeftCount(str, pad, count) {
    while (str.length < count) {
        str = pad + str;
    }
    return str;
}

function padLeft(obj, pad) {
    var sPad = '';
    if (obj.value.length > 0) {
        while ((sPad.length + obj.value.length) < obj.maxLength) {
            sPad += pad;
        }
        obj.value = sPad + obj.value;
    }
}

function SoloNumeros(myfield, e) {
    try
    {
        var keycode;
        if (!window.event.ctrlKey) {
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            else return true;
            if (((keycode > 47) && (keycode < 58)) || (keycode == 8)) { return true; }
            else
                return false;
        }
        else
            return false;
    }
    catch (e) {
        return false;
    }
}

function getIndex(input) {
    var index = -1, i = 0, found = false;
    while (i < input.form.length && index == -1)
        if (input.form[i] == input) index = i;
    else i++;
    return index;
}
function nextIndexEnabled(input, i) {
    var index = i + 1;
    while (input.form[index].disabled ||
                   input.form[index].type == 'radio')
        index++;
    return index;
}
function autoTab(input, e, maxFiels) {
    try
    {
        //var x = document.getElementById('<%=txtAnio.ClientID%>');
        var keyCode = (isNN) ? e.which : e.keyCode;
        var filter = (isNN) ? [0, 8, 9] : [0, 8, 9, 16, 17, 18, 37, 38, 39, 40, 46];
        var nameLastFiel = "";
        switch (maxFiels) {            
            case 2:
                nameLastFiel = 'txtMes';
                break;
            case 3:
                nameLastFiel = 'txtAnio';
            case 0:
                nameLastFiel = 'txtDigito';
            break;
        }
        switch(keyCode)
        {
        case 8 : 
            if ((input.id.indexOf('txtDia') == -1) &&
                isStartPos(input)) {
                var obj = input.form[getIndex(input) - 1];
                obj.focus();
                setEnd(obj);
            }
        break; 
        case 37 : 
            if (input.id.indexOf('txtDia') == -1) {
                if (isStartPos(input)) {
                    var obj = input.form[getIndex(input) - 1];
                    obj.focus();
                    setEnd(obj);
                }
            }
        break;
        case 39 :
            if (input.id.indexOf(nameLastFiel) == -1) {
                if (isEndPos(input)) {
                    input.value = input.value.slice(0, input.maxLength);
                    var i = nextIndexEnabled(input, getIndex(input));
                    input.form[i].focus();
                    //setEnd(input);
                    return true;
                }
            }
            break;
        case 110:
            if (input.id.indexOf(nameLastFiel) == -1) {
                if (input.value == '')
                    input.value = 0;
                input.form[nextIndexEnabled(input, getIndex(input))].focus();
                return true;
            }        
        default :
            if (input.value.length >= input.maxLength && !containsElement(filter, keyCode)) {
                
                input.value = input.value.slice(0, input.maxLength);
                if (input.id.indexOf(nameLastFiel) == -1)
                {
                    input.form[nextIndexEnabled(input, getIndex(input))].focus();
                    return true;
                }
                if (input.id.indexOf(nameLastFiel) != -1 &&
                    input.form[nextIndexEnabled(input, getIndex(input))].id.indexOf('txtDia') != -1) {
                    input.form[nextIndexEnabled(input, getIndex(input))].focus();
                    return true;
                }
            }
        break;
        }
    }
    catch (e) {
        return false;
    }
}

function IsBeginOrEnd(o) {
    var FieldRange = o.createTextRange();
    FieldRange.moveStart('character', o.value.length);
    return 0;
}

function setEnd(o) {
    var FieldRange = o.createTextRange();
    FieldRange.moveStart('character', o.value.length);
    FieldRange.collapse();
    FieldRange.select();
}

function containsElement(arr, ele) {
    var found = false, index = 0;
    while (!found && index < arr.length)
        if (arr[index] == ele)
        found = true;
    else
        index++;
    return found;
}

function isStartPos(textElement) {
    //save off the current value to restore it later,
    textElement.maxLength = textElement.maxLength + 1;
    var sOldText = textElement.value;

    //create a range object and save off it's text
    var objRange = document.selection.createRange().duplicate();
    var sOldRange = objRange.text;

    //set this string to a small string that will not normally be encountered
    var sWeirdString = '#';

    //insert the weirdstring where the cursor is at
    objRange.text = sOldRange + sWeirdString;
    objRange.moveStart('character', (0 - sOldRange.length - sWeirdString.length));

    //save off the new string with the weirdstring in it
    textElement.maxLength = textElement.maxLength - 1;
    var sNewText = textElement.value;

    //set the actual text value back to how it was
    objRange.text = sOldRange;

    return (sNewText.indexOf(sWeirdString) == 0);
}

function isEndPos(textElement) {
    textElement.maxLength = textElement.maxLength + 1;
    
    var sOldText = textElement.value;
    var objRange = document.selection.createRange().duplicate();
    var sOldRange = objRange.text;
    var sWeirdString = '#';

    objRange.text = sOldRange + sWeirdString;
    objRange.moveStart('character', (0 - sOldRange.length - sWeirdString.length));
    textElement.maxLength = textElement.maxLength - 1;
    
    var sNewText = textElement.value;
    objRange.text = sOldRange;

    return (sNewText.indexOf(sWeirdString) == textElement.value.length);
}


//AGREGADO SERGIO
function containsElementControl(arr, ele) {
    var found = false, index = 0;
    while (!found && index < arr.length)
        if (arr[index] == ele)
        found = true;
    else
        index++;
    return found;
}

function getIndexControl(input) {
    var index = -1, i = 0, found = false;
    while (i < input.form.length && index == -1)
        if (input.form[i] == input) index = i;
    else i++;
    return index;
}

function autoTabControl(input, len, e) {
    var keyCode = (isNN) ? e.which : e.keyCode;
    var filter = (isNN) ? [0, 8, 9] : [0, 8, 9, 16, 17, 18, 37, 38, 39, 40, 46];
    if (input.value.length >= len && !containsElementControl(filter, keyCode)) {
        input.value = input.value.slice(0, len);
        input.form[(getIndexControl(input) + 1) % input.form.length].focus();

        return true;
    }
}

function autoTabControl_conDestino(input, len, e, idObjDestino) {
    var keyCode = (isNN) ? e.which : e.keyCode;
    var filter = (isNN) ? [0, 8, 9] : [0, 8, 9, 16, 17, 18, 37, 38, 39, 40, 46];
    if (input.value.length >= len && !containsElementControl(filter, keyCode)) {
        input.value = input.value.slice(0, len);
        document.getElementById(idObjDestino).focus();
        return true;
    }
}

function validarNumeroControl(e) {
    tecla_codigo = (document.all) ? e.keyCode : e.which;
    if (tecla_codigo == 8) return true;
    patron = /[0-9]/;
    tecla_valor = String.fromCharCode(tecla_codigo);
    return patron.test(tecla_valor);
}

function IsNumeric(sText) {
  
    patron = /[0-9]/;
    return patron.test(sText);
}

// no pongo el event listener porque es un fucking kilombo
function separaDatos(evt, obj) {

    // este flag sirve para que cuando se inserte el substring en los != txts no se ejecute el metodo
    //        if (isExecuting_separaDatos)
    //            return;

    isExecuting_separaDatos = true;
    var text = window.clipboardData.getData("Text").trim();
    var cuil = text.replace("-", "");
    while (cuil != cuil.replace("-", ""))
        cuil = cuil.replace("-", "");

    if (!IsNumeric(cuil) || cuil.length != 11)
        return;

    // document.getElementById('<% = txtCodigo.ClientID %>').value = cuil.substring(0, 2);
    // document.getElementById('<% = txtNumero.ClientID %>').value = cuil.substring(2, 10);
    // document.getElementById('<% = txtDigito.ClientID %>').value = cuil.substring(10, 11);

    $('#' + obj.id).find(':input')[0].value = cuil.substring(0, 2);
    $('#' + obj.id).find(':input')[1].value = cuil.substring(2, 10);
    $('#' + obj.id).find(':input')[2].value = cuil.substring(10, 11);
    //isExecuting_separaDatos = false;
}

function maskFecha(objeto) {

    var allowedArray = ['0','1','2','3','4','5','6','7','8','9'];
    var x = String.fromCharCode(event.keyCode);
    var encontrado = false;
    for (var i in allowedArray)
        if (allowedArray[i] == x)
            encontrado = true;

    if (!encontrado)
        return false;
    /*
    if (!
           (event.keyCode == 8 ||
            event.keyCode == 9 ||
            event.keyCode == 13 ||
            event.keyCode == 35 ||
            event.keyCode == 36 ||
            event.keyCode == 46 ||
           (event.keyCode >= 48 && event.keyCode <= 57) ||
           (event.keyCode >= 96 && event.keyCode <= 105) ||
           (event.keyCode >= 37 && event.keyCode <= 40)))
        return false;
    */
    if (event.keyCode != 8) {
        if (objeto.value.length == 2 || objeto.value.length == 5) {
            objeto.value += "/";
        }
    }
    return true;
}

function jsDecimals(evt, input) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 45
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;

    
    if (key >= 48 && key <= 57 || key == 45) {
            
            if (key == 45)
               return true ;
            else 
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            if (key == 8 || key == 13 || key == 0) {
                return true;
            } else if (key == 46) {
                if (filter(tempValue) === false) {
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }    
}
function filter(__val__) {
    var preg = /^([-;0-9]+\.?[0-9]{0,2})$/;
    if (preg.test(__val__) === true) {
        return true;
    } else {
        return false;
    }

}



