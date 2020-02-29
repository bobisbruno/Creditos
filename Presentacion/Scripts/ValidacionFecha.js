/*
    Validacion de una fecha separada en 3 campos
*/
//Funcion para ASP.NET CustomValidator
function Custom3FieldsDateValidator(oSrc, args)
{
    var nDay = document.getElementById(sFieldNameDate1).value;
    var nMonth = document.getElementById(sFieldNameDate2).value;
    var nYear = document.getElementById(sFieldNameDate3).value;    
    args.IsValid = isDate(nMonth + "/" + nDay + "/" + nYear);
}

/**
 * DHTML date validation script. Courtesy of SmartWebby.com (http://www.smartwebby.com/dhtml/)
 */
// Declaring valid date character, minimum year and maximum year
    var dtCh= "/";
    var minYear=1900;
    var maxYear=2100;

    function isInteger(s){
	    var i;
        for (i = 0; i < s.length; i++){   
            // Check that current character is number.
            var c = s.charAt(i);
            if (((c < "0") || (c > "9"))) return false;
        }
        // All characters are numbers.
        return true;
    }

    function stripCharsInBag(s, bag){
	    var i;
        var returnString = "";
        // Search through string's characters one by one.
        // If character is not in bag, append to returnString.
        for (i = 0; i < s.length; i++){   
            var c = s.charAt(i);
            if (bag.indexOf(c) == -1) returnString += c;
        }
        return returnString;
    }

    function daysInFebruary (year){
	    // February has 29 days in any year evenly divisible by four,
        // EXCEPT for centurial years which are not also divisible by 400.
        return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
    }
    function DaysArray(n) {
	    for (var i = 1; i <= n; i++) {
		    this[i] = 31
		    if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		    if (i==2) {this[i] = 29}
       } 
       return this
    }
    function isDate(dtStr){
	    var daysInMonth = DaysArray(12)
	    var pos1=dtStr.indexOf(dtCh)
	    var pos2=dtStr.indexOf(dtCh,pos1+1)
	    var strMonth=dtStr.substring(0,pos1)
	    var strDay=dtStr.substring(pos1+1,pos2)
	    var strYear=dtStr.substring(pos2+1)
	    strYr=strYear
	    if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	    if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	    for (var i = 1; i <= 3; i++) {
		    if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	    }
	    month=parseInt(strMonth)
	    day=parseInt(strDay)
	    year=parseInt(strYr)
	    if (pos1==-1 || pos2==-1){
		    //alert("The date format should be : mm/dd/yyyy")
		    return false
	    }
	    if (strMonth.length<1 || month<1 || month>12){
		    //alert("Please enter a valid month")
		    return false
	    }
	    if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		    //alert("Please enter a valid day")
		    return false
	    }
	    if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		    //alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear)
		    return false
	    }
	    if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		    //alert("Please enter a valid date")
		    return false
	    }
    return true
    }

    function ValidateForm(){
	    var dt=document.frmSample.txtDate
	    if (isDate(dt.value)==false){
		    dt.focus()
		    return false
	    }
        return true
     }
function finMes(oTxt)
{
	var nMes = parseInt(oTxt.substr(3, 2), 10);
	var nRes = 0;
	switch (nMes){
		case 1: nRes = 31; break;
		case 2: nRes = 29; break;
		case 3: nRes = 31; break;
		case 4: nRes = 30; break;
		case 5: nRes = 31; break;
		case 6: nRes = 30; break;
		case 7: nRes = 31; break;
		case 8: nRes = 31; break;
		case 9: nRes = 30; break;
		case 10: nRes = 31; break;
		case 11: nRes = 30; break;
		case 12: nRes = 31; break;
	}
	return nRes;
}

function valDia(oTxt)
{
	var bOk = true;
	var nDia = oTxt.substr(0, 2);
	for (var i = 0; i < nDia.length; i++)
	{
		bOk = bOk && esDigito(nDia.charAt(i));
	}
	if (bOk)
		bOk = bOk && ((nDia >= 1) && (nDia <= finMes(oTxt)));
	return bOk;
}

function daysInFebruary (oTxt){
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    var nMes = oTxt.substr(3, 2);
    var nDia = oTxt.substr(0, 2);
    var nAno = oTxt.substr(6);
    
    if (nMes=="02")
		if  (nDia<=
         ((nAno % 4 == 0) && !(nAno % 100 == 0) ? 29 : 28 ))
         return true;
         else
         return false;
        else
        return true;
}

function valMes(oTxt)
{
	var bOk = true;
	var nMes = oTxt.substr(3, 2);
	for (var i = 0; i < nMes.length; i++)
	{
		bOk = bOk && esDigito(nMes.charAt(i));
	}
	if (bOk)
		bOk = bOk &&  ((nMes >= 1) && (nMes <= 12));

	return bOk;
}
function esDigito(sChr) {
    var sCod = sChr.charCodeAt(0);
    return ((sCod > 47) && (sCod < 58));
}

function valSep(oTxt) {
    var bOk = false;
    bOk = bOk || ((oTxt.charAt(2) == "-") && (oTxt.charAt(5) == "-"));
    bOk = bOk || ((oTxt.charAt(2) == "/") && (oTxt.charAt(5) == "/"));
    return bOk;
}
function valAno(oTxt)
{
	
	var bOk = true;
	var nAno = oTxt.substr(6);
	bOk = bOk && (nAno.length == 4);
	if (bOk)
	{
		for (var i = 0; i < nAno.length; i++)
		{
			bOk = bOk && esDigito(nAno.charAt(i));
		}
	}
	return bOk;
}

function valFecha(oTxt){
	var bOk = false;
	if (oTxt != "--")
	{
		bOk = true;
		bOk = bOk && (valAno(oTxt));
		if (bOk)
			bOk = bOk && (valMes(oTxt));
		if (bOk)			
			bOk = bOk && (valDia(oTxt));
		if (bOk)			
			bOk = bOk && (valSep(oTxt));
		if (bOk)			
			bOk = bOk && daysInFebruary(oTxt)
	}
	return bOk;
}

function valFechaFull(oTxt) {
    var bOk = false;
    if (oTxt.length==10) {
        bOk = true;
        bOk = bOk && (valAno(oTxt));
        if (bOk)
            bOk = bOk && (valMes(oTxt));
        if (bOk)
            bOk = bOk && (valDia(oTxt));
        if (bOk)
            bOk = bOk && (valSep(oTxt));
        if (bOk)
            bOk = bOk && daysInFebruary(oTxt)
    }
    return bOk;
}


function FechaFutura(oTxt)
{
var nAno = (oTxt.substr(6));
var nMes = (oTxt.substr(3, 2));
var nDia = (oTxt.substr(0, 2));
var ingresada=new Date(nAno,nMes,nDia);

var actual=new Date();
var dia= actual.getDate();
var mes= actual.getMonth()+1;
var anio= actual.getFullYear();
var hoy=new Date(anio,mes,dia);


if (ingresada>hoy)
    return false;
 else
    return true;
}
function FechaAnteriorOctubreVieja(oTxt)
{
var nAno = (oTxt.substr(6));
var nMes = (oTxt.substr(3, 2));
var nDia = (oTxt.substr(0, 2));

if (nAno>2004)
	return false;
else
	if (nAno<2004)
		return true;
	else
		if (nMes >10)
			return false;
		else
			if (nMes<10)
				return true;
			else
				return false;
					
					
		
}
			
function ObtenerFecha()
{
	return document.getElementById(txtDia).value + "-" +  document.getElementById(txtMes).value + "-" + document.getElementById(txtAnio).value;
}

     
function FechaValida(source, arguments)
{
	var Fecha=ObtenerFecha();
	arguments.IsValid=valFecha(Fecha);
}

function FechaAnteriorOctubre(source, arguments)
{
	var Fecha=ObtenerFecha();
	if (valFecha(Fecha))
		arguments.IsValid=!FechaAnteriorOctubreVieja(Fecha);
	else
		arguments.IsValid=true;
}

function FechaPosteriorAhoy(source, arguments)
{
	var Fecha=ObtenerFecha();
	if (valFecha(Fecha))
		arguments.IsValid=FechaFutura(Fecha);
	else
		arguments.IsValid=true;
}

function PeriodoValido(source, arguments) 
{
    var FechaDesde = document.getElementById(txtFechaDesde).value;
    var FechaHasta = document.getElementById(txtFechaHasta).value;


    arguments.IsValid = (FechaDesde.length == 0 && FechaHasta.length == 0);

    if (FechaDesde.length > 0) {
        if (valFechaFull(FechaDesde)) {
            if (FechaHasta.length > 0) {
                if (valFechaFull(FechaHasta)) {

                    var d1 = FechaDesde.substring(0, 2);
                    var m1 = FechaDesde.substring(3, 5);
                    var a1 = FechaDesde.substring(6, 10);

                    var d2 = FechaHasta.substring(0, 2);
                    var m2 = FechaHasta.substring(3, 5);
                    var a2 = FechaHasta.substring(6, 10);

                    var f1 = new Date(a1, m1+1, d1);
                    var f2 = new Date(a2, m2+1, d2);
                    arguments.IsValid = (f1 <= f2);
                } //valFechaFull
            } //fechaHasta.Length
            else
                arguments.IsValid = true;
        }//valFechaFull
    }//FechaDesde
    else
    {
        if (FechaHasta.length > 0) 
            arguments.IsValid = (valFechaFull(FechaHasta)) 
     }
            
        

}

function FechaPosterioraHoy(source, arguments) {

    arguments.IsValid = true;

    var FechaDesde = document.getElementById(txtFechaDesde).value;

    if (FechaDesde.length > 0) {
        if (valFechaFull(FechaDesde)) {
            var actual = new Date();
            var dia = actual.getDate();
            var mes = actual.getMonth() + 1;
            var anio = actual.getFullYear();
            var hoy = new Date(anio, mes, dia);

            var d1 = FechaDesde.substring(0, 2);
            var m1 = FechaDesde.substring(3, 5);
            var a1 = FechaDesde.substring(6, 10);
            var f1 = new Date(a1, m1, d1);
            if (f1 < hoy)
                arguments.IsValid = false;
        }
    }
}

function ValidarAgencia(source, arguments) {

    var Agencia = document.getElementById(lblAgencia).value;
    arguments.IsValid=(Agencia != "");

}
