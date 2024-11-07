using System.Net;

namespace acentenoS6B.Views;

public partial class vInsertarEstudiante : ContentPage
{
	public vInsertarEstudiante()
	{
		InitializeComponent();
	}

    private void btnGuardar_Clicked(object sender, EventArgs e)
    {
		try
		{
			WebClient cliente = new WebClient();	
			var parametros =new System.Collections.Specialized.NameValueCollection();
			parametros.Add("nombre",txtNombre.Text);
			parametros.Add("apellido",txtApellido.Text);
			parametros.Add("edad",txtEdador.Text);

			cliente.UploadValues("http://192.168.17.48/estudiantesws/estudiant.php", "POST", parametros);
			Navigation.PushAsync(new vEstudiante());

		}
		catch(Exception ex)
		{
			DisplayAlert("Error", ex.Message, "ok");
		}
			
    }
}