var isNN = (navigator.appName.indexOf("Netscape") != -1);

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

function autoTabControl(input, e, len) {
    try {
        var keyCode = (isNN) ? e.which : e.keyCode;
        var filter = (isNN) ? [0, 8, 9] : [0, 8, 9, 16, 17, 18, 37, 38, 39, 40, 46];

        if (keyCode == 8) {
            if (isStartPos(input)) {
                var obj = input.form[getIndexControl(input) - 1];

                obj.focus();
                setEnd(obj);

            }
        }
        else if (keyCode == 86 || keyCode == 45) {

            var ss = 0;
            var text = window.clipboardData.getData("Text");

            if (input.form[getIndexControl(input)].id.substring(13, input.form[getIndexControl(input)].id.length) == "txtCodigo" ||
                input.form[getIndexControl(input)].id.substring(14, input.form[getIndexControl(input)].id.length) == "txtDia") {
                ss = getIndexControl(input);
            }
            else if (input.form[getIndexControl(input) - 1].id.substring(13, input.form[getIndexControl(input) - 1].id.length) == "txtCodigo" ||
                input.form[getIndexControl(input) - 1].id.substring(14, input.form[getIndexControl(input) - 1].id.length) == "txtDia") {
                ss = getIndexControl(input) - 1;
            }
            else if (input.form[getIndexControl(input) - 2].id.substring(13, input.form[getIndexControl(input) - 2].id.length) == "txtCodigo" ||
                input.form[getIndexControl(input) - 2].id.substring(14, input.form[getIndexControl(input) - 2].id.length) == "txtDia") {
                ss = getIndexControl(input) - 2;
            }

            if (input.form[ss].id.substring(13, input.form[ss].id.length) == "txtCodigo") {
                if (text.length == 11) {
                    if (esNumero(text) != "") {
                        input.form[ss].value = text.substring(0, 2);
                        input.form[ss + 1].value = text.substring(2, 10);
                        input.form[ss + 2].value = text.substring(10, 11);
                    }
                    else {
                        input.form[ss].value = "";
                        input.form[ss + 1].value = "";
                        input.form[ss + 2].value = "";
                    }
                }
                else if (text.length == 13) {
                    var v1 = esNumero(text.substring(0, 2));
                    var v2 = esNumero(text.substring(3, 11));
                    var v3 = esNumero(text.substring(12, 13));

                    if (v1 == "" || v2 == "" || v3 == "") {
                        input.form[ss].value = "";
                        input.form[ss + 1].value = "";
                        input.form[ss + 2].value = "";

                    }
                    else {
                        input.form[ss].value = v1;
                        input.form[ss + 1].value = v2;
                        input.form[ss + 2].value = v3;
                    }

                }
            }
            else if (input.form[ss].id.substring(14, input.form[ss].id.length) == "txtDia") {
                if (text.length == 8) {
                    input.form[ss].value = esNumero(text.substring(0, 2));
                    input.form[ss + 1].value = esNumero(text.substring(2, 4));
                    input.form[ss + 2].value = esNumero(text.substring(4, 8));
                }
                else if (text.length == 10) {
                    input.form[ss].value = esNumero(text.substring(0, 2));
                    input.form[ss + 1].value = esNumero(text.substring(3, 5));
                    input.form[ss + 2].value = esNumero(text.substring(6, 10));
                }
            }

        }
        else if (input.value.length >= len && !containsElementControl(filter, keyCode)) {
            input.value = input.value.slice(0, len);
            input.form[(getIndexControl(input) + 1) % input.form.length].focus();

            return true;
        }
    }
    catch (Error)
    { }
}

function setEnd(o) {
    var FieldRange = o.createTextRange();
    FieldRange.moveStart('character', o.value.length);
    FieldRange.collapse();
    FieldRange.select();
}

function isStartPos(textElement) {
    try {

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

        return (sNewText.indexOf(sWeirdString) == 0);
    }
    catch (Error) {
        if (textElement.value == "") {
            return true;
        } else { return false; }
    }
}

function validarNumeroControl(e) {
    tecla_codigo = (document.all) ? e.keyCode : e.which;
    if (tecla_codigo == 8) return true;
    patron = /[0-9]/;
    tecla_valor = String.fromCharCode(tecla_codigo);
    return patron.test(tecla_valor);
}

function esNumero(text) {
    rexp = new RegExp('(^[0-9]*$)');
    splitText = rexp.exec(text);

    if (splitText != null) {
        return text;
    }
    else {
        return "";
    }
}

function maskFecha(objeto) {

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

    if (event.keyCode != 8) {
        if (objeto.value.length == 2 || objeto.value.length == 5) {
            objeto.value += "/";
        }
    }
    return true;
}

function separaDatos(evt, obj) {

    isExecuting_separaDatos = true;
    var text = window.clipboardData.getData("Text").trim();
    var cuil = text.replace("-", "");
    while (cuil != cuil.replace("-", ""))
        cuil = cuil.replace("-", "");

    if (!esNumero(cuil) || cuil.length != 11)
        return;

    $('#' + obj.id).find(':input')[0].value = cuil.substring(0, 2);
    $('#' + obj.id).find(':input')[1].value = cuil.substring(2, 10);
    $('#' + obj.id).find(':input')[2].value = cuil.substring(10, 11);
}