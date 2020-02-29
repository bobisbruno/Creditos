function CuilNoNumerico2(cuil)			
{
	
		for (i=0; i < cuil.length; i++) {   
			caracter=cuil.charAt(i);
			if ( caracter.charCodeAt(0) >= 48 && caracter.charCodeAt(0) <= 57 )     {
	            
			}
			else
				{
					return false;
				}
		}
		return true;
   
    
}
function  CuilIncorrecto2(cuil) {
        x=i=dv=0;
        vec=new Array(10);
        // Multiplico los dígitos.
        vec[0] = cuil.charAt(  0) * 5;
        vec[1] = cuil.charAt(  1) * 4;
        vec[2] = cuil.charAt(  2) * 3;
        vec[3] = cuil.charAt(  3) * 2;
        vec[4] = cuil.charAt(  4) * 7;
        vec[5] = cuil.charAt(  5) * 6;
        vec[6] = cuil.charAt(  6) * 5;
        vec[7] = cuil.charAt(  7) * 4;
        vec[8] = cuil.charAt(  8) * 3;
        vec[9] = cuil.charAt(  9) * 2;
                    
        // Suma cada uno de los resultado.
        for( i = 0;i<=9; i++) {
            x += vec[i];
        }
        dv = (11 - (x % 11)) % 11;
        if ( dv == cuil.charAt( 10) ) { 
            return true;
        } else{
			return false;
			
        }
}



function ObtenerCUIL()
{

	return document.getElementById(txtCuitPre).value + 
	document.getElementById(txtCuitDoc).value +
	document.getElementById(txtCuitDV).value;
}




function ObtenerCUILA()
{
	return	(document.getElementById(txtCuilPreA).value +	document.getElementById(txtCuilDocA).value +	document.getElementById(txtCuilDVA).value);
}

	

function CuilANoValido(source, arguments)
{
	var cuil=ObtenerCUILA();
	if (cuil !=null && cuil.length>0)
		arguments.IsValid=(CuilNoNumerico2(cuil) && (cuil.length == 11) && CuilIncorrecto2(cuil));
	else
		arguments.IsValid=true;
}
function CuilNoNumerico(source, arguments)
{
	var cuil=ObtenerCUIL();
	if (cuil.length == 11)
	    arguments.IsValid = (CuilNoNumerico2(cuil));
	else
	    arguments.IsValid = true;
}

/*2era Validacion*/
function CuilIncompleto(source, arguments)
{
    var cuil = ObtenerCUIL();
    if (cuil.length > 0) {
        if (CuilNoNumerico2(cuil))
            arguments.IsValid = (cuil.length == 11);
        else
            arguments.IsValid = true;
    }
    else
        arguments.IsValid = true;
}

/*3era Validacion*/
function CuilIncorrecto(source, arguments)
{
	var cuil=ObtenerCUIL();
	if (cuil.length == 11 && CuilNoNumerico2(cuil))
		arguments.IsValid= CuilIncorrecto2(cuil);
	else
		arguments.IsValid=true;			
}


