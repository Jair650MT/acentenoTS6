using acentenoS6B.Models;
using Newtonsoft.Json;
using System.Text;

namespace acentenoS6B.Views;

public partial class vActualizarEliminar : ContentPage
{
	public vActualizarEliminar(Estudiante datos )
	{
		InitializeComponent();
        txtCodigo.Text=datos.codigo.ToString();
        txtNombre.Text=datos.nombre.ToString();
        txtApellido.Text=datos.apellido.ToString();
        txtEdad.Text=datos.edad.ToString();	
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmación", "¿Está seguro de que desea actualizar los datos del estudiante?", "Sí", "No");
        if (!confirm)
            return;

        try
        {
            var estudianteActualizado = new
            {
                codigo = int.Parse(txtCodigo.Text),
                nombre = txtNombre.Text,
                apellido = txtApellido.Text,
                edad = int.Parse(txtEdad.Text)
            };

            var jsonData = JsonConvert.SerializeObject(estudianteActualizado);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (var cliente = new HttpClient())
            {
                var response = await cliente.PutAsync("http://192.168.1.39/estudiantews/post.php", content);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Estudiante actualizado exitosamente.", "OK");
                    await Navigation.PopAsync(); // Regresa a la lista de estudiantes
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo actualizar el estudiante.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar este estudiante?", "Sí", "No");
        if (!confirm)
            return;

        try
        {
            int codigo = int.Parse(txtCodigo.Text); // Obtener el código desde el campo de texto
            using (var cliente = new HttpClient())
            {
                var response = await cliente.DeleteAsync($"http://192.168.1.39/estudiantews/post.php?codigo={codigo}");

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Estudiante eliminado exitosamente.", "OK");
                    await Navigation.PopAsync(); // Navega de regreso después de eliminar
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo eliminar el estudiante: {errorContent}", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

}