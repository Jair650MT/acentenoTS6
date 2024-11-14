using acentenoS6B.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace acentenoS6B.Views;

public partial class vEstudiante : ContentPage
{
	private const string Url = "http://192.168.1.39/estudiantews/post.php";
	private readonly HttpClient cliente= new HttpClient();
	private ObservableCollection<Models.Estudiante> estud;
	public vEstudiante()
	{
		InitializeComponent();
		listar();

    }

	public async void listar()
	{
		var content =await cliente.GetStringAsync(Url);
		List<Models.Estudiante>listEst= JsonConvert.DeserializeObject<List<Estudiante>>(content);
		estud=new ObservableCollection<Models.Estudiante>(listEst);
        lvEstudiantes.ItemsSource =estud;
	}

    private void btnInsertar_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new Views.vInsertarEstudiante());
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        listar(); // Vuelve a cargar la lista de estudiantes
    }
    private async void Button_Editar_Clicked(object sender, EventArgs e)
    {
        var estudiante = (sender as Button).CommandParameter as Estudiante;
        if (estudiante != null)
        {
            await Navigation.PushAsync(new vActualizarEliminar(estudiante)); // Navega a la página de actualización
        }
    }

    private async void Button_Eliminar_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar este estudiante?", "Sí", "No");
        if (!confirm)
            return;

        // Obtener el código del estudiante
        int codigo = (int)(sender as Button).CommandParameter;

        using (var cliente = new HttpClient())
        {
            // URL de la solicitud DELETE con el parámetro de código
            var response = await cliente.DeleteAsync($"http://192.168.1.39/estudiantews/post.php?codigo={codigo}");

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Estudiante eliminado exitosamente.", "OK");
                listar(); // Recarga la lista de estudiantes después de eliminar
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"No se pudo eliminar el estudiante: {errorContent}", "OK");
            }
        }
    }
}