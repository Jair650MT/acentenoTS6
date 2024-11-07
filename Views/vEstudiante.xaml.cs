using acentenoS6B.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace acentenoS6B.Views;

public partial class vEstudiante : ContentPage
{
	private const string Url = "http://192.168.17.48/estudiantesws/estudiant.php";
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
}