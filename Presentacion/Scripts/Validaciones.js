
function ObtenerTelefono() {
    return (document.getElementById(txtTelediscado).value + document.getElementById(txtTelefono).value);
}
function ObtenerMail() {
    return (document.getElementById(txtEmail).value);
}
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

//function autoTab(input, len, e) {
//    var keyCode = (isNN) ? e.which : e.keyCode;
//    var filter = (isNN) ? [0, 8, 9] : [0, 8, 9, 16, 17, 18, 37, 38, 39, 40, 46];
//    if (input.length >= len && !containsElement(filter, keyCode)) {
//        input.value = input.slice(0, len);
//        input.form[(input.tabIndex + 1)].focus();
//        return true;
//    }
//}

function keyPressed(e, idBoton) {
    if (window.event.keyCode == 13) {
        enterPressedOnTextBox(idBoton);
        return false;
    } 
    else {
        return validarNumero(e);
    }
}

function enterPressedOnTextBox(idBoton) {
    var boton = document.getElementById(idBoton);
    boton.focus();
    boton.click();
}

function validarNumero(e) {
    try {
        tecla_codigo = (document.all) ? e.keyCode : e.which;
        if (tecla_codigo == 8) return true;
        patron = /[0-9]/;
        tecla_valor = String.fromCharCode(tecla_codigo);
        return patron.test(tecla_valor);
    }
    catch (e) {
        return false;
    }
}

function valPeriod(sDate, esFechaHasta) {
    var arrDate = "";
    if (sDate.length >= 6)
        arrDate = (sDate).replace(/[.]/g, "/").split('/');
    else
        arrDate = ("01/" + sDate).replace(/[.]/g, "/").split('/');

    var d = arrDate[0];
    var m = arrDate[1];
    var a = arrDate[2];

    if (parseInt(a) > 1599 && parseInt(a) < 2999) {
        if (parseInt(m, 10) > 0 && parseInt(m, 10) < 13) {
            if (esFechaHasta)
                arrDate[0] = daysInMonth(m, a);
            else
                arrDate[0] = "01";
            arrDate[1] = (m.length == 1 ? '0' + m : m);
            return arrDate;

        }
        else
            return null;
    }
    else
        return null;
}

function isCuit(source, args) {
    var suma;
    var resto;
    var verif;
    nro = args.Value;

    var pos = nro.split("");
    if (!/^\d{11}$/.test(nro)) {
        args.IsValid = false;
        return false;
    }
    var y = 0;
    while (y < pos.length) {
        suma = (pos[0] * 5 + pos[1] * 4 + pos[2] * 3 + pos[3] * 2 + pos[4] * 7 +
                pos[5] * 6 + pos[6] * 5 + pos[7] * 4 + pos[8] * 3 + pos[9] * 2);
        resto = suma % 11;
        if (resto == 0) {
            verif = 0;
            break;
        }
        else if (resto == 1 && (pos[1] == 0 || pos[6] == 7)) {
            pos[1] = 4;
            continue;
        }
        else {
            verif = 11 - resto;
            break;
        }
        y += 1;
    }
    args.IsValid = (pos[10] == verif);
    return;
}

function ValidarDatoContacto(source, arguments) {
    var email = ObtenerMail();
    var telefono = ObtenerTelefono();

    arguments.IsValid = !(email.length == 0 && telefono.length == 0);
}

function ObtenerCUIL() {
    return document.getElementById(txtCuitPre).value + document.getElementById(txtCuitDoc).value + document.getElementById(txtCuitDV).value;
}

function CuilIncompleto(source, arguments) {
    var cuil = ObtenerCUIL();

    arguments.IsValid = (cuil.length == 11);
}

function getSelectedText() {
    var txt = '';
    if (window.getSelection) {
        txt = window.getSelection();
    }
    else if (document.getSelection) {
        txt = document.getSelection();
    }
    else if (document.selection) {
        txt = document.selection.createRange().text;
    }
    return txt;
}

function ProximoCampo(oEvent, sFieldID, sNextFieldID, sSubmitBtnId, tipo) {
    var browser = navigator.appName;
    var elEvento = oEvent || window.event;
    var codigoTecla = 0;
    //Determino la tecla que presiono segun el explorador

    if (browser == "Microsoft Internet Explorer") {
        codigoTecla = elEvento.keyCode;
    }
    else {
        if (elEvento.charCode != 0) {
            codigoTecla = elEvento.charCode;
        } //ES FF
        else { return; } // PRESIONO UNA TECLA NO VALIDA
    }
    var letraTecla = String.fromCharCode(codigoTecla);

    if (codigoTecla == 13)
        document.getElementById(sSubmitBtnId).click();

    //Si no es un numero (ASCII >= 48 y <=57
    if (tipo == null)
        return true;

    if (tipo == "int" && (codigoTecla < 48 || codigoTecla > 57)) {
        if (browser == "Microsoft Internet Explorer") {
            elEvento.keyCode = null;
        }
        return false;
    }
    if (tipo == "Fecha" && (codigoTecla < 47 || codigoTecla > 57)) {
        if (browser == "Microsoft Internet Explorer") {
            elEvento.keyCode = null;
        }
        return false;
    }
    if (tipo == "Decimal" && (codigoTecla < 48 || codigoTecla > 57) && codigoTecla != 44) {
        if (browser == "Microsoft Internet Explorer") {
            elEvento.keyCode = null;
        }
        return false;
    }


    try {
        var s = getSelectedText();
        var oField = document.getElementById(sFieldID);

        if (s != undefined && s.length > 0) oField.value = "";


        if (oField.maxLength == 1) {
            oField.value = letraTecla;
            var oField = document.getElementById(sNextFieldID);
            oField.focus();
            //oField.select();
            elEvento.keyCode = null;
        }
        else if (oField.value.length == oField.maxLength - 1) {
            oField.value += letraTecla;
            var oField = document.getElementById(sNextFieldID);
            oField.focus();
            oField.select();
            elEvento.keyCode = null;

        }
        else if (oField.value.length > oField.maxLength - 1) {
            oField.value = "";
            //oField.value = letraTecla;
            //var oField = document.getElementById(sNextFieldID);
            oField.focus();
            //oField.select();
            //elEvento.keyCode = null;
        }

    } catch (e) { var oField = ""; }
}
