using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2Ejercicio2_3
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImagenListViewPage : ContentPage
	{
		public ImagenListViewPage(List<PM2Ejercicio2_3Data.Imagen> imagenes)
		{
			InitializeComponent ();
            imagenListView.ItemsSource = imagenes;
        }
	}
}