using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


// La información general sobre un ensamblado se controla mediante el siguiente 
// conjunto de atributos. Cambie estos atributos para modificar la información
// asociada con un ensamblado.
[assembly: AssemblyTitle("AplicacionWebTest")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AplicacionWebTest")]
[assembly: AssemblyCopyright("Copyright ©  2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Si establece ComVisible como false, los tipos de este ensamblado no estarán visibles 
// para los componentes COM. Si necesita obtener acceso a un tipo de este ensamblado desde 
// COM, establezca el atributo ComVisible como true en este tipo.
[assembly: ComVisible(false)]

// El siguiente GUID sirve como identificador de typelib si este proyecto se expone a COM
[assembly: Guid("3d5900ae-111a-45be-96b3-d9e4606ca793")]

// La información de versión de un ensamblado consta de los cuatro valores siguientes:
//
//      Versión principal
//      Versión secundaria 
//      Número de versión de compilación
//      Revisión
//
// Puede especificar todos los valores o establecer como predeterminados los números de versión de revisión y compilación 
// mediante el asterisco ('*'), como se muestra a continuación:
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyFileVersion("1.0.0.0")]

public class AssemblyInfo
{
    public AssemblyInfo(){} 

    public static string dateversionnumbers()
    {
        const string carrReturn = "<br />";
        string sbuild = string.Empty;

        sbuild += " ******Server****** " + carrReturn;
        sbuild += " server: [" + System.Environment.MachineName + "] " + carrReturn;
        sbuild += " os: [" + System.Environment.OSVersion + "] " + carrReturn;
        sbuild += " hostname: [" + System.Net.Dns.GetHostName() + "] " + carrReturn;
        sbuild += " ip: [" + System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString() + "] " + carrReturn;
        sbuild += " current date: [" + System.DateTime.Now + "] " + carrReturn;
        sbuild += carrReturn;


        System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
        sbuild += " ******Memory Assembly****** " + carrReturn;
        sbuild += " - nombre: [" + ass.GetName().Name + "] " + carrReturn;
        sbuild += " - version: [" + ass.GetName().Version + "] " + carrReturn;
        sbuild += " - build date: [" + new System.DateTime(2000, 1, 1).AddDays(ass.GetName().Version.Build).AddSeconds(ass.GetName().Version.Revision * 2) + "] " + carrReturn;
        sbuild += carrReturn;

        System.IO.FileInfo oMyFile = new
        System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        System.Reflection.Assembly a = System.Reflection.Assembly.LoadFrom(System.Reflection.Assembly.GetExecutingAssembly().Location);
        sbuild += " ******Physical Assembly****** " + carrReturn;
        sbuild += " - nombre: [" + oMyFile.Name + "] " + carrReturn;
        sbuild += " - version: [" + a.GetName().Version + "] " + carrReturn;
        sbuild += " - creation date : [" + oMyFile.CreationTime + "] " + carrReturn;
        sbuild += " - modification date : [" + oMyFile.LastWriteTime + "] " + carrReturn;
        sbuild += carrReturn;

        return sbuild;
    }
}