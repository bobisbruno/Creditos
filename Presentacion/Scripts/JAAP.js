
	var isNN = (navigator.appName.indexOf("Netscape")!=-1);	
	
	function Limpiar()
	{
		for( i=1; i<10; i++)
		{
				document.getElementById("imgVal" + i).src = "Imagenes/icon_checkmark1.gif";
				document.getElementById("txtCuitPre" + i).value='';			
				document.getElementById("txtCuitDoc" + i).value='';			
				document.getElementById("txtCuitDV" + i).value='';			
		}
		document.getElementById("lblMensaje").value='';
	}
		
	function autoTab(input,len, e) 
			{
				var keyCode = (isNN) ? e.which : e.keyCode; 
				var filter = (isNN) ? [0,8,9] : [0,8,9,16,17,18,37,38,39,40,46];
				if(input.value.length >= len && !containsElement(filter,keyCode)) {
					input.value = input.value.slice(0, len);
					input.form[(getIndex(input)+1) % input.form.length].focus();
				return true;
				}
			}

	function seleccion(input, len)
	{
		input.select();
	}

	function SoloNumeros(){
			if (event.keyCode < 48 || event.keyCode > 57){
				
					return false;
			}
			else
			{
				return true;
			}
		}
		
	function containsElement(arr, ele) {
			var found = false, index = 0;
			while(!found && index < arr.length)
				if(arr[index] == ele)
					found = true;
				else
					index++;
			return found;
			}

			function getIndex(input) {
			var index = -1, i = 0, found = false;
			while (i < input.form.length && index == -1)
				if (input.form[i] == input)index = i;
				else i++;
			return index;
			}