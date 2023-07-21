using System;
using System.IO;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using PM2Ejercicio2_3.Models;
using PM2Ejercicio2_3Data;
using Xamarin.Forms.Shapes;
using Path = System.IO.Path;

namespace PM2Ejercicio2_3
{
    public partial class MainPage : ContentPage
    {
        private string filePath; // Variable para almacenar la ruta de la foto capturada
        private MediaFile photo; // Variable para almacenar la foto capturada
        private SQLiteHelper dbHelper;

        public MainPage()
        {
            InitializeComponent();
            string dbPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db3");
            dbHelper = new SQLiteHelper(dbPath1);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verificar si se otorgó el permiso de cámara
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Error", "La cámara no está disponible.", "OK");
                    return;
                }

                // Solicitar permisos para acceder a la cámara
                var status = await CrossMedia.Current.Initialize();
                if (!status)
                {
                    await DisplayAlert("Permiso denegado", "No se ha otorgado el permiso para acceder a la cámara.", "OK");
                    return;
                }

                // Tomar una foto
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "CapturedPhotos",
                    Name = "capturedImage.jpg",
                    SaveToAlbum = true // Guardar la foto en el álbum de fotos del dispositivo (opcional)
                });

                if (photo != null)
                {
                    // Obtener la ruta de la foto capturada
                    filePath = photo.Path;

                    // Asignar la imagen a la vista Image
                    imageField.Source = ImageSource.FromFile(filePath);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                await DisplayAlert("Error", $"Ha ocurrido un error: {ex.Message}", "OK");
            }
        }

        private async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (photo != null && !string.IsNullOrEmpty(descriptionEntry.Text))
                {
                    // Convertir la imagen a byte array
                    byte[] imageBytes;
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            imageBytes = memoryStream.ToArray();
                        }
                    }

                    // Guardar en la base de datos SQLite
                    var imagen = new Imagen
                    {
                        ImageName = "capturedImage.jpg",
                        ImageData = imageBytes,
                        Description = descriptionEntry.Text // Guardar la descripción ingresada en el Entry
                    };

                    await dbHelper.InsertImagenAsync(imagen); // Insertar la imagen en la base de datos

                    // Notificar que la imagen ha sido guardada
                    await DisplayAlert("Éxito", "La imagen y la descripción han sido guardadas en la base de datos.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No se ha capturado ninguna imagen o no has ingresado una descripción.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ha ocurrido un error al guardar la imagen: {ex.Message}", "OK");
            }
        }




        private async void VerImagenes_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Obtener la lista de imágenes desde la base de datos
                var imagenes = await dbHelper.GetImagenesAsync();

                // Crear una nueva instancia de la página ListView y pasarle la lista de imágenes como parámetro
                var listViewPage = new ImagenListViewPage(imagenes);

                // Navegar a la página ListView
                await Navigation.PushAsync(listViewPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ha ocurrido un error al obtener las imágenes: {ex.Message}", "OK");
            }
        }




    }
}
