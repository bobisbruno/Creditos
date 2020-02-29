

function Browser()
{
  var ua, s, i;
  this.isIE    = false;
  this.isNS    = false;
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

function SelectAllCheck(obj) {
    for (var i = 1; i < $('form input:checkbox').length; i++) {
        var e = $('form input:checkbox')[i];
        if ((e.name != 'allbox') && (e.type == 'checkbox') && (!e.disabled)) {
            e.checked = true;  //!e.checked;
            if(obj.type == 'checkbox' && !obj.checked)
                e.checked = false;
        }
    }
}

/* Funciones para strings */
function keyToUpper(e) {
    tecla_codigo = (document.all) ? e.keyCode : e.which;
    return String.fromCharCode(tecla_codigo).toUpperCase();
}
function trim(stringToTrim) {
    return stringToTrim.replace(/^\s+|\s+$/g,"");
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
/* FIN Funciones para strings FIN */
    

function Cerrar()
{
    ventana=window.parent.self; 
    ventana.opener=window.parent.self; 
    ventana.close();
}
function showWait()
{
  document.body.style.cursor = 'wait';
}