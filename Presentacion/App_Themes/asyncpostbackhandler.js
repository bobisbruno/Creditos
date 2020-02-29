/*
 Fueled by $3b@
 Version:1 [at] 30-10-08
*/

//add handlers
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(requestEndHandler );
Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(requestInitializeHandler );
//Sys.WebForms.PageRequestManager.getInstance().add_pageLoading(loadingPageHandler );

// add error message if doesn't exists
if (!document.getElementById("errorMessageLabel"))
	document.write("<div id='errorMessageLabel' align='center' style='color:red;'></div>");

var divMsg="<div id='msgPageRequestManagerHandler' style='";
divMsg += "position:absolute; display:none; ";
//divMsg += "border:2px solid black;";
//divMsg += "background-color:#f4f6e1;";
divMsg += "width:175px; height:70px;";
//divMsg += "color:Gray; font-size:12px;";
divMsg += "'><br/><br/>Cargando, Aguarde...</div>";

var divBck = "<div id='capaPageRequestManagerHandler' style='";
divBck += "position:absolute;";
divBck += "display:none;";

/* negro */
/**/
divBck += "filter:alpha(opacity=35);";
divBck += "opacity:0.35;";
divBck += "background-color:black;";


/* transparente 
divBck += "filter:alpha(opacity=0);";
divBck += "opacity:0;";
divBck += "background-color:white;";
*/
divBck += "'></div>";

//add divs 4 waiting message
document.write(divMsg);	
document.write(divBck);

// This function will handle the end request event
function requestEndHandler(sender, args) {
    hideHandler();
    if (args.get_error()) {
        document.getElementById("errorMessageLabel").innerText =
        //"Error: " + args.get_error().description;
                               "INTENTE NUEVAMENTE LA OPERACION";
        args.set_errorHandled(true);
    }
    else {
        document.getElementById("errorMessageLabel").innerText = "";
        if (typeof aspbh_endHandler != "undefined") {
            if (typeof aspbh_endHandler == "function")
                aspbh_endHandler();
            else {
                for (h in aspbh_endHandler) {
                    aspbh_endHandler[h]();
                }
            }
        
        }
            
    }
}


function requestInitializeHandler(sender, args) {

    // si tengo elementos para excepcion
    if (typeof aspbh_exceptions != "undefined" && aspbh_exceptions.length > 0) {
        // instancio el manager
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        //if (prm.get_isInAsyncPostBack())
        var idObjPostback = args.get_postBackElement().id;

        //me fijo si el que disparo el evento es una excepcion
        for (var i = 0; i < aspbh_exceptions.length; i++) {
            //si es excepcion => return
            if (idObjPostback == aspbh_exceptions[i]) {
                return;
            }
        }
    }


    dysplayHandler();
}

function loadingPageHandler(sender, args)
{
	// no se usa mas aca porque si da error no llega
	hideHandler();
}

function hideHandler()
{
	document.getElementById("capaPageRequestManagerHandler").style.display = "none";
	document.getElementById("msgPageRequestManagerHandler").style.display = "none";
}
function dysplayHandler()
{

	var capa = document.getElementById("capaPageRequestManagerHandler");
	var msg = document.getElementById("msgPageRequestManagerHandler");
	
	capa.style.position ="absolute";
	capa.style.display = "block";

	capa.style.left = 0;
	capa.style.top = 0;
    
	msg.style.position ="absolute";
	msg.style.left = document.body.clientWidth/2 - (msg.style.width.replace("px","")/2)-100 +"px";
	//msg.style.top = document.body.clientHeight/2 - (msg.style.height.replace("px","")/2) + "px";
	msg.style.top = document.documentElement.scrollTop + document.body.scrollTop + document.body.clientHeight/2 - (msg.style.height.replace("px","")/2) + "px";
	msg.style.display = "block";
	
	var tmp = getMaxZindex();
	capa.style.zIndex= tmp+1;
	msg.style.zIndex=tmp+2;

	//Detecto la version del navegador y ajusto la capa del fondo
	var browser = new Browser();
	
	if (browser.version <7 && browser.isIE)
	{
		capa.style.width = window.screen.width;
		//capa.style.height = window.screen.height ; //screen.height;
		capa.style.height = document.documentElement.scrollTop + document.body.scrollTop + document.body.clientHeight;
	}
	else {
	    
		capa.style.width = 100 + "%";
		//capa.style.height = 200 + "%";		
		capa.style.height = document.documentElement.scrollTop + document.body.scrollTop + document.body.clientHeight + "px";
	}
			
	/*
	//oculto los combos y listas para el ie6 el mensaje no quede abajo de ellos
	for (var i = 0, j = document.getElementsByTagName('select').length; i < j; i++)
	{
		document.getElementsByTagName('select')[i].style.display="none";
	}
	*/
}

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

function getMaxZindex()
{
    var allElems;
    try
    {
        if (document.getElementsByTagName("*"))
            allElems = document.getElementsByTagName("*");
        else
            allElems = document.all; // or test for that too
    }
    catch (err)
    {
        return 5000;
    }
    
    var maxZIndex = 0;
    for(var i=0;i<allElems.length;i++) {
        var elem = allElems[i];
        
        if(elem.style.zIndex== null)
            return 5000;
            
        maxZIndex=Math.max(maxZIndex,Number(elem.style.zIndex));
        //alert(maxZindex);        
        /*
        var elem = allElems[i];
        var cStyle = null;
        if (elem.currentStyle) {cStyle = elem.currentStyle;}
        else if (document.defaultView && document.defaultView.getComputedStyle)
        {
            cStyle = document.defaultView.getComputedStyle(elem,"");
        }
        var sNum;
        if (cStyle) {
            sNum = Number(cStyle.zIndex);
        } else {
            sNum = Number(elem.style.zIndex);
        }
        if (!isNaN(sNum)) {
            maxZIndex = Math.max(maxZIndex,sNum);
        }
        */
    }
    return maxZIndex;
}