using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace acentenoS6B.Views;

public partial class vInsertarEstudiante : ContentPage
{
    public vInsertarEstudiante()
    {
        InitializeComponent();
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        // Validación de campos
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtEdad.Text) ||
            !int.TryParse(txtEdad.Text, out int edad))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos correctamente.", "OK");
            return;
        }

        try
        {
            // Crear el objeto estudiante y convertirlo a JSON
            var nuevoEstudiante = new
            {
                nombre = txtNombre.Text,
                apellido = txtApellido.Text,
                edad = edad
            };

            var jsonData = JsonConvert.SerializeObject(nuevoEstudiante);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (var cliente = new HttpClient())
            {
                var response = await cliente.PostAsync("http://192.168.1.39/estudiantews/post.php", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Estudiante guardado exitosamente.", "OK");
                    await Navigation.PopAsync(); // Vuelve a la página principal después de guardar
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo guardar el estudiante: {errorContent}", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
